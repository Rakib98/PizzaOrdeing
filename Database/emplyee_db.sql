DROP DATABASE if EXISTS employee_db;
CREATE DATABASE employee_db;
USE employee_db;

CREATE TABLE tbl_login(
	ID INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
	username VARCHAR(30) NOT NULL,
    pass VARCHAR(25) NOT NULL,
    employee_id VARCHAR(10) NOT NULL,
    UNIQUE(employee_id)
);

INSERT INTO tbl_login VALUES(null, 'JonDoe1', 'password', 'XAA1057412');
INSERT INTO tbl_login VALUES(null, 'Jared20', 'password2', 'XAA6122008');