using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderingApp
{
    public partial class Form1 : Form
    {
        //Variables for the database connection
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        bool connectionSuccess;

        //EmployeeID to store, and pass to the other form
        string EmployeeID;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //assigns values to database, and set connection
            server = "localhost";
            database = "employee_db";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

           
            connectionSuccess = OpenConnection();
            if (connectionSuccess == true)
            {
                //MessageBox.Show("Connection to database is successful.");
                CloseConnection();
                   
            }
            else
            {
                //Show an error message
                MessageBox.Show("Can't connect to database.");
                //Closes the application
                this.Close();
            }
            

        }

        private bool OpenConnection()
        {
            //Trys to open a conenction with the database
            try
            {
                //Opens connection to database
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //Catches the error, in case it is 0 the connection failed, in case is 1045 the username or password are invalid
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.");
                        break;
                    case 1045:
                        MessageBox.Show("Username or password for database are invalid");
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection()
        {
            //Closes the connection with the database
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            //Sets variables to user input
            string userInput = txtUsername.Text;
            string passInput = txtPassword.Text;
            //Bool to check password
            bool match = false;

            //opens connection
            OpenConnection();
            string queryUser = "SELECT * FROM tbl_login WHERE username ='" + userInput + "';"; //Query that will be executed
            MySqlCommand cmd = new MySqlCommand(queryUser, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                //Try to check the password with the username, if they don't match an error is showed
                while (dataReader.Read() && match == false)
                {
                    if (dataReader["pass"].ToString() == passInput)
                    {
                        //Gets the employee id from the database, and stores it in a variable.
                        EmployeeID = dataReader["employee_id"].ToString();
                        //Hides this form, open the order form and closes the login one.
                        this.Hide();
                        Form order = new order(EmployeeID); //Adds the variable employeeID to the new form, so that it can pass the value
                        order.Closed += (s, args) => this.Close();
                        order.Show();
                    }
                    else if (dataReader["pass"].ToString() != passInput)
                    {
                        MessageBox.Show("Error; deatils incorrect");
                    }
                }
            }
            catch (MySqlException ex)
            {
                //If there is an SQL error it will be displayed
                MessageBox.Show(ex.Message);
            }
            CloseConnection();
        }
    }
}
