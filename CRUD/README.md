CREATE DATABASE CRUDops;
use CRUDops;

CREATE TABLE customers (
id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
firstname VARCHAR(100) NOT NULL,
lastname VARCHAR(100) NOT NULL,
email VARCHAR(100) NOT NULL UNIQUE,
phone VARCHAR(100) NOT NULL,
address VARCHAR(100) NOT NULL,
company TEXT NOT NULL,
created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO customers (firstname, lastname, email, phone, address, company) VALUES
('John', 'Doe', 'john.doe@example.com', '123-456-7890', '123 Elm St, Springfield', 'Acme Corp'),
('Jane', 'Smith', 'jane.smith@example.com', '234-567-8901', '456 Oak St, Springfield', 'Globex Corp'),
('Mike', 'Johnson', 'mike.johnson@example.com', '345-678-9012', '789 Pine St, Springfield', 'Initech'),
('Emily', 'Davis', 'emily.davis@example.com', '456-789-0123', '101 Maple St, Springfield', 'Hooli'),
('Sarah', 'Wilson', 'sarah.wilson@example.com', '567-890-1234', '202 Birch St, Springfield', 'Umbrella Corp'),
('David', 'Brown', 'david.brown@example.com', '678-901-2345', '303 Cedar St, Springfield', 'Vandelay Industries'),
('Laura', 'Jones', 'laura.jones@example.com', '789-012-3456', '404 Spruce St, Springfield', 'Stark Industries'),
('Chris', 'Garcia', 'chris.garcia@example.com', '890-123-4567', '505 Cherry St, Springfield', 'Wayne Enterprises'),
('Jessica', 'Martinez', 'jessica.martinez@example.com', '901-234-5678', '606 Walnut St, Springfield', 'Griffin Industries'),
('Brian', 'Anderson', 'brian.anderson@example.com', '012-345-6789', '707 Chestnut St, Springfield', 'Blue Sun Corp');

DESCRIBE customers;

ALTER TABLE customers MODIFY COLUMN Createdat DATETIME;
