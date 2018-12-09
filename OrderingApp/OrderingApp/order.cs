using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderingApp
{
    public partial class order : Form
    {
        /*================================CREATE GLOBAL VARIABLES=========================*/
        //Employee ID for reference
        string employeeID;

        //List of all the items that will be added. This will be used to constantly display all the items selected.
        List<string> totalOrders = new List<string>();

        //Create a list of pizzas, this will contain just the pizzas added
        List<pizza> pizzaAndTopping = new List<pizza>();

        //List of drinks
        List<drink> drinks = new List<drink>();

        //List of sides
        List<side> sides = new List<side>();

        //variable that assigns a price to a single topping. Having the price in a variable is easier.
        //As in the future the price might change, so only one variable will need to be changed, instead of few lines code.
        double singleToppingPrice = 0.80;
        //variable for a single soft drink
        double singleDrink = 0.70;

        //Create a variable to store how much the first deal saves
        double dealOneSaves = 0;

        //Create a variable to store how much the second deal saves
        double dealTwoSaves = 0;

        //Variable to set the delivery charge
        double deliveryCharge = 2;

        //Variable for stroing the regualr total
        double totalReg = 0;

        //Variable to store the final total
        double totFinal = 0;

        //Customer info
        string name;
        string address;
        string postcode;
        string phone;
        

        public order(string employeeID_ref)
        {
            InitializeComponent();
            //initialsie the form with a string, and sets the string as the employeeID from the database.
            employeeID = employeeID_ref;
        }

        private void order_Load(object sender, EventArgs e)
        {
            //Display the employee ID in the text box when the form loads. The ID has been transferred from the login form, to here
            //This has been done so that this form, wouldn't need to have all the code to conenct to the database.
            txtEmployeeID.Text = employeeID;

            //Hides the checkout page, so that it can only be seen when all the items have been added
            tcOrder.TabPages.Remove(tpCheckout);

            timer1.Start();
        }

        /*===============================NEXT AND PREVIOUS TAB INTERACTION=======================*/
        private void GoToNextTab()
        {
            //Method that will be used to every next button, to go in the next tab

            //Gets current index of the tab page, and adds one
            int i = tcOrder.SelectedIndex + 1;
            //Chancghes the index, so that it can go to the next page
            tcOrder.SelectedIndex = i;
        }

        /*======================================METHOD FOR POPULATING LISTS============================*/
        private void pizzaListShow()
        {
            //method for showing pizza in the listbox
            if (pizzaAndTopping.Count > 0)
            {
                lstPizza.Items.Clear();
                for (int i = 0; i < pizzaAndTopping.Count; i++)
                {
                    lstPizza.Items.Add(pizzaAndTopping[i].size + " Toppings x" + pizzaAndTopping[i].numOfToppings + " " + $"{pizzaAndTopping[i].price:C2}");
                }
            }
        }

        private void sideListShow()
        {
            //method for showing sides in the listbox
            if (sides.Count > 0)
            {
                lstSides.Items.Clear();
                for (int i = 0; i < sides.Count; i++)
                {
                    lstSides.Items.Add(sides[i].name + " Quantity x" + sides[i].quantity + " " + $"{sides[i].price:C2}");
                }
            }
        }

        private void drinkListShow()
        {
            //method for showing drinks in the listbox
            if (drinks.Count > 0)
            {
                lstDrinks.Items.Clear();
                for (int i = 0; i < drinks.Count; i++)
                {
                    lstDrinks.Items.Add(drinks[i].name + " Quantity x" + drinks[i].quantity + " " + $"{drinks[i].price:C2}");
                }
            }
        }

        private void ShowListPizzaTopping()
        {
            //Method that is called to populate the list in the checkout that shows all the pizzas with the selected toppings
            //For loop to cycle through the pizzas, and display the size.
            lstPizzaAndToppings.ClearSelected();
            for (int i = 0; i < pizzaAndTopping.Count(); i++)
            {
                lstPizzaAndToppings.Items.Add(i.ToString() + ")" + " " + pizzaAndTopping[i].size + " Pizza with: ");

                //for loop to cycle through the topping list. It takes the index of the pizza used (from the for loop before)
                //uses it as the reference to target the right topping list; and then cycle through the topping list to display all the topping, of a pizza.
                for (int a = 0; a < pizzaAndTopping[i].toppings.Count; a++)
                {
                    lstPizzaAndToppings.Items.Add("      -" + pizzaAndTopping[i].toppings[a]);
                }
            }
        }

        /*====================================DEALS=========================================*/
        public double ApplyDealOne()
        {
            //Method that will be called, when the right checkbox is selected

            //Counter, to check if the deal can be applied multiple times
            int counter1 = 0;
            //Cycle through each pizza object
            for (int i = 0; i < pizzaAndTopping.Count; i++)
            {
                //Check if a medium pizza has 4 toppings
                if (pizzaAndTopping[i].size == "Medium (£ 5)" && pizzaAndTopping[i].numOfToppings == 4 && (drinks.Count > 0))
                {
                    counter1++;
                    dealOneSaves = counter1 * 2.90;
                }
            }
            //If the amount saved is zero, it gives a messagge
            if (dealOneSaves == 0)
            {
                MessageBox.Show("Deal can't be applied");
                //Uncheck the checbox
                cbDeal1.Checked = false;
            }
            //Return the amount saved.
            return dealOneSaves;
        }

        public double ApplyDealTwo()
        {
            //Method that will be called, when the right checkbox is selected

            //Counter, to check if the deal can be applied multiple times
            int counter2 = 0;

            //Counter to check if there are 2 large pizzas with four toppings in the list
            int counterLargePizza = 0;
            //Cycle through each pizza object
            for (int i = 0; i < pizzaAndTopping.Count; i++)
            {
                //Check if a medium pizza has 4 toppings
                if (pizzaAndTopping[i].size == "Large (£ 7)" && pizzaAndTopping[i].numOfToppings == 4)
                {
                    //Increase the number of large pizzas
                    counterLargePizza++;

                    //Checks if the number of large pizzas is a multiple of 2. So that the order can be applied for all large pizzas with 4 toppings.
                    //And not just the first one in the list
                    if ((counterLargePizza % 2) == 0)
                    {
                        counter2++;
                        dealTwoSaves = counter2 * 0.21;
                    }
                }
            }
            //If the amount saved is zero, it gives a messagge
            if (dealTwoSaves == 0)
            {
                MessageBox.Show("Deal can't be applied");
                //Unchecks the checkbox
                cbDeal2.Checked = false;
            }
            //Return the amount saved.
            return dealTwoSaves;
        }

        /*==============================================CALCULATIONS=====================================*/
        private void CalculateTotal()
        {
            /*=======================Calculate the total price for the order.=====================*/

            //Calculate total price for the sides
            double totalPriceSide = 0;
            foreach (side s in sides)
            {
                totalPriceSide += s.price;
            }

            //Calculate total price for the drinks
            double totPriceDrink = 0;
            foreach (drink d in drinks)
            {
                totPriceDrink += d.price;
            }

            //Calculate total price for the pizzas
            double totPricePizza = 0;
            foreach (pizza p in pizzaAndTopping)
            {
                totPricePizza += p.price;
            }

            //Display the regular price
            totalReg = totalPriceSide + totPriceDrink + totPricePizza;
            txtRegPrice.Text = $"{totalReg:C2}";

            //Display delivery charges
            txtDeliveryCharges.Text = $"{2:C2}";

            //Display tot price
            totFinal = totalReg - (dealOneSaves + dealTwoSaves) + deliveryCharge;
            txtTotPrice.Text = $"{totFinal:C2}";

        }

        /*===========================================ADD PIZZA==========================================*/
        private void btnAddPizza_Click(object sender, EventArgs e)
        {
            //Create a new pizzza item
            pizza newPizza = new pizza();
            try
            {
                newPizza.size = cbSize.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("Select a pizza size");
            }
            // Adding toppings selected. The if statements check if the checkbox is checked,
            // if it is checked it adds the name of the topping to the list of toppings that is inside the pizza class.
            if (clbTopping.GetItemCheckState(0) == CheckState.Checked)
            {
              newPizza.toppings.Add("Anchovies");
            }
            if (clbTopping.GetItemCheckState(1) == CheckState.Checked)
            {
              newPizza.toppings.Add("Black Olives");
            }
            if (clbTopping.GetItemCheckState(2) == CheckState.Checked)
            {
               newPizza.toppings.Add("Peppers");
            }
            if (clbTopping.GetItemCheckState(3) == CheckState.Checked)
            {
               newPizza.toppings.Add("Jalapenos");
            }
            if (clbTopping.GetItemCheckState(4) == CheckState.Checked)
            {
                newPizza.toppings.Add("Mushrooms");
            }
            if (clbTopping.GetItemCheckState(5) == CheckState.Checked)
            {
               newPizza.toppings.Add("Red onions");
            }
            if (clbTopping.GetItemCheckState(6) == CheckState.Checked)
            {
                newPizza.toppings.Add("Sweetcorn");
            }
            if (clbTopping.GetItemCheckState(7) == CheckState.Checked)
            {
              newPizza.toppings.Add("Pepperoni");
            }
            if (clbTopping.GetItemCheckState(8) == CheckState.Checked)
            {
                newPizza.toppings.Add("Pineapple");
            }
            if (clbTopping.GetItemCheckState(9) == CheckState.Checked)
            {
                newPizza.toppings.Add("Spicy beef");
            }
            if (clbTopping.GetItemCheckState(10) == CheckState.Checked)
            {
               newPizza.toppings.Add("Chicken");
            }
            if (clbTopping.GetItemCheckState(11) == CheckState.Checked)
            {
               newPizza.toppings.Add("Sausages");
            }
            if (clbTopping.GetItemCheckState(12) == CheckState.Checked)
            {
                newPizza.toppings.Add("Ham");
            }
            if (clbTopping.GetItemCheckState(13) == CheckState.Checked)
            {
               newPizza.toppings.Add("Tuna");
            }
            //Counts the number of toppings checked, so that it can be used to calculate the total price, as each topping has the same price
            newPizza.numOfToppings = newPizza.toppings.Count;
            //Calucalting the price of the pizza
            double basePizza = 0;
            //Gives a price for the pizza without toppings
            if (cbSize.SelectedIndex == 0)
            {
                basePizza = 3.50;
            }
            else if (cbSize.SelectedIndex == 1)
            {
                basePizza = 5;
            }
            else if (cbSize.SelectedIndex == 2)
            {
                basePizza = 7;
            }
            //Price of the pizza with the toppings
            newPizza.price = basePizza + (newPizza.numOfToppings * singleToppingPrice);
            //Add the new object to the pizza list
            pizzaAndTopping.Add(newPizza);
            pizzaListShow();

            //For loop to uncheck every checkbox in the topping checked list box.
            for (int i = 0; i < clbTopping.Items.Count; i++)
            {
                clbTopping.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void btnGoToSides_Click(object sender, EventArgs e)
        {
            GoToNextTab();
        }

        /*=================================================ADD SIDES===============================*/
        private void btnAddSide_Click(object sender, EventArgs e)
        {
            //Clears the list of sides, every time the add button is clicked. It will avoid duplicate.
            sides.Clear();

            //side newSide = new side();
            if (Convert.ToInt32(nudPlainGarlic.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "Plain garlic bread";
                newSide.quantity = Convert.ToInt32(nudPlainGarlic.Value);
                newSide.price = 1.70 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nudCheeseGarlic.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "Cheese garlic bread";
                newSide.quantity = Convert.ToInt32(nudCheeseGarlic.Value);
                newSide.price = 2.20 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nud5SpicyChicken.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "5 Spicy chicken wings";
                newSide.quantity = Convert.ToInt32(nud5SpicyChicken.Value);
                newSide.price = 3.50 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nud10SpicyChicken.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "10 Spicy chicken wings";
                newSide.quantity = Convert.ToInt32(nud10SpicyChicken.Value);
                newSide.price = 6 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nudFries.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "Regular fries";
                newSide.quantity = Convert.ToInt32(nudFries.Value);
                newSide.price = 1.00 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nudLargeFries.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "Large fries";
                newSide.quantity = Convert.ToInt32(nudLargeFries.Value);
                newSide.price = 1.30 * newSide.quantity;
                sides.Add(newSide);
            }
            if (Convert.ToInt32(nudColeslaw.Value) > 0)
            {
                side newSide = new side();
                newSide.name = "Coleslaw";
                newSide.quantity = Convert.ToInt32(nudColeslaw.Value);
                newSide.price = 0.70 * newSide.quantity;
                sides.Add(newSide);
            }

            //Calls the method to display items in the side listbox
            sideListShow();
        }

        private void btnGoToDrink_Click(object sender, EventArgs e)
        {
            GoToNextTab();
        }

        /*=========================================ADD DRINKS=======================================*/
        private void btnAddDrink_Click(object sender, EventArgs e)
        {
            //Clears the drink list to avoid duplicate
            drinks.Clear();

            if (Convert.ToInt32(nudCoke.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "Coke";
                newDrink.quantity = Convert.ToInt32(nudCoke.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            if (Convert.ToInt32(nudPepsi.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "Pepsi";
                newDrink.quantity = Convert.ToInt32(nudPepsi.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            if (Convert.ToInt32(nudDiet.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "Diet Coke";
                newDrink.quantity = Convert.ToInt32(nudDiet.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            if (Convert.ToInt32(nudSevenUp.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "7-Up";
                newDrink.quantity = Convert.ToInt32(nudSevenUp.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            if (Convert.ToInt32(nudFanta.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "Fanta";
                newDrink.quantity = Convert.ToInt32(nudFanta.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            if (Convert.ToInt32(nudTango.Value) > 0)
            {
                drink newDrink = new drink();
                newDrink.name = "Tango";
                newDrink.quantity = Convert.ToInt32(nudTango.Value);
                newDrink.price = singleDrink * newDrink.quantity;
                drinks.Add(newDrink);
            }
            drinkListShow();
        }

        private void btnGoToCustomer_Click(object sender, EventArgs e)
        {
            GoToNextTab();
        }

        /*================================================CHECKOUT================================*/
        private void btnGoToCheckout_Click(object sender, EventArgs e)
        {
            //Adds the checkout page
            tcOrder.TabPages.Add(tpCheckout);
            //Calls the next tab method, to get to the next tab
            GoToNextTab();
            //Calls the method to populate the list in the checkout page.
            ShowListPizzaTopping();
            //Calls the method that contains all the calculations
            CalculateTotal();
        }


        private void cbDeal1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDeal1.Checked == true)
            {
                ApplyDealOne();
            }
            txtDeal.Text = $"{(dealOneSaves + dealTwoSaves).ToString():C2}";

            //Display tot price after the deal has been selected
            totFinal = totalReg - (dealOneSaves + dealTwoSaves) + deliveryCharge;
            txtTotPrice.Text = $"{totFinal:C2}";
        }

        private void cbDeal2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDeal2.Checked == true)
            {
                ApplyDealTwo();
            }
            txtDeal.Text = $"{(dealOneSaves + dealTwoSaves).ToString():C2}";

            //Display tot price after the deal has been selected
            totFinal = totalReg - (dealOneSaves + dealTwoSaves) + deliveryCharge;
            txtTotPrice.Text = $"{totFinal:C2}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Calls the methods to populate the various lists after a certain amount of time passed. This allow to automatically update the lists,
            //without the need of user interactions. Also it allows to reduce how many times the methods are called, so it makes the code a bit neater.
            pizzaListShow();
            drinkListShow();
            sideListShow();
            ShowListPizzaTopping();
            CalculateTotal();
        }

        /*================ Delte buttons on tab pages==============*/
        private void btlDeletePizza_Click(object sender, EventArgs e)
        {
            pizzaAndTopping.RemoveAt(lstPizza.SelectedIndex);
        }

        private void btnDeleteSide_Click(object sender, EventArgs e)
        {
            sides.RemoveAt(lstSides.SelectedIndex);
        }

        private void btnDeleteDrink_Click(object sender, EventArgs e)
        {
            drinks.RemoveAt(lstDrinks.SelectedIndex);
        }

        /*================ validating text boxes===================*/
        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            Regex RegexName = new Regex(@"^w+$"); //Regex that will be compared to the name input, so that only letters, and spaces can be inserted
            //Checks if regex matches name
            if (RegexName.IsMatch(txtName.ToString()))
            {
                name = txtName.ToString();
            }
            else if (!RegexName.IsMatch(txtName.ToString()))
            {
                MessageBox.Show("Name field can contain only letters, spaces, hypens and apostrophes");
                txtName.Clear();
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            Regex RegexAddress = new Regex(@"^[a-zA-Z0-9\, ]+$"); // Regex that will be compared to the address input, will allow only letters, numbers spaces, comas.

            //Checks if regex matches address
            if (RegexAddress.IsMatch(txtAddress.ToString()))
            {
                address = txtAddress.ToString();
            }
            else if (!RegexAddress.IsMatch(txtAddress.ToString()))
            {
                MessageBox.Show("Address field can contain only letters, numbers, spaces and comas");
                txtAddress.Clear();
            }
        }

        private void txtPost_Validating(object sender, CancelEventArgs e)
        {
            Regex RegexPostcode = new Regex(@"^[a-zA-Z0-9\ ]$"); // Regex that will be compared to the postcode input, it will allow only letters, numbers, spaces. Max char is 7

            //Checks if the regex matches the postcode
            if (RegexPostcode.IsMatch(txtPost.ToString()))
            {
                postcode = txtPost.ToString();
            }
            else if (!RegexPostcode.IsMatch(txtPost.ToString()))
            {
                MessageBox.Show("Postcode field can contain only letters, numbers and spaces");
                txtPost.Clear();
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            Regex RegexPhone = new Regex(@"^[0-9\]+$"); // Regex that will be compared to the phone number input, so that only numbers will be accepted. 

            //Checks if regex matches phone
            if (RegexPhone.IsMatch(txtPhone.ToString()))
            {
                phone = txtPhone.ToString();
            }
            else if (!RegexPhone.IsMatch(txtPhone.ToString()))
            {
                MessageBox.Show("Phone field can contain only numbers");
                txtPhone.Clear();
            }
        }
    }

    class pizza
    {
        //Creates class containing the size, price, the total number of topping selected, and the toppin chosen
        public string size;
        public List<string> toppings = new List<string>();
        public int numOfToppings;
        public double price;
    }
    class drink
    {
        public string name;
        public double price;
        public int quantity;
    }
    class side
    {
        public string name;
        public double price;
        public int quantity;
    }
}