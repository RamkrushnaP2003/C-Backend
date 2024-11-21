CREATE DATABASE Task;
use Task;

CREATE TABLE employees (
	id INT AUTO_INCREMENT PRIMARY KEY,
    firstname VARCHAR(30) NOT NULL,
    lastname VARCHAR(30) NOT NULL,
    email VARCHAR(30) NOT NULL,
    department VARCHAR(20) NOT NULL
);

DELIMITER //

CREATE PROCEDURE AddEmployee (
	IN firstName VARCHAR(30),
    IN lastName VARCHAR(30),
    IN email VARCHAR(30),
    IN department VARCHAR(20)
)

BEGIN 
	INSERT INTO employees (firstname, lastname, email, department)
    VALUES (firstName, lastName, email, department);
END 

// DELIMITER ;

-- Create the managers table
CREATE TABLE managers (
	id INT AUTO_INCREMENT PRIMARY KEY,
    firstname VARCHAR(30) NOT NULL,
    lastname VARCHAR(30) NOT NULL,
    email VARCHAR(30) NOT NULL,
    department VARCHAR(30) NOT NULL, -- Corrected to 30 to match the stored procedure
    teamsize INT NOT NULL
);

-- Change the delimiter so that the procedure body is treated as a single unit
DELIMITER //

-- Create the stored procedure
CREATE PROCEDURE AddManager (
	IN firstName VARCHAR(30),
    IN lastName VARCHAR(30),
    IN email VARCHAR(30),
    IN department VARCHAR(30),
    IN teamSize INT
)
BEGIN
	INSERT INTO managers (firstname, lastname, email, department, teamsize)
    VALUES (firstName, lastName, email, department, teamSize);
END //

-- Restore the delimiter to the default semicolon
DELIMITER ;

DELIMITER //

CREATE PROCEDURE GetAllEmployees()
BEGIN
    SELECT firstName, lastName, email, department
    FROM employees;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE GetAllManagers()
BEGIN
    SELECT FirstName, LastName, Email, Department, TeamSize
    FROM managers;
END //

DELIMITER ;

CALL GetAllManagers();

