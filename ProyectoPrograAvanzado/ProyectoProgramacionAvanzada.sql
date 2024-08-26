CREATE DATABASE ProyectoProgramacionAvanzada;
Use ProyectoProgramacionAvanzada;


Create table Products  (
	Id int primary key Identity(1,1),
	Name varchar(50),
	Price decimal(10,2),
	Details nvarchar(max)
);
 
 
Create table ProductImages (
	Id int primary key identity(1,1),
	Id_Product  int,
	Image_Url varchar(255),
	Name varchar(255),
	constraint fk_Product_Product_Images foreign key (Id_Product) references Products (Id)
);


INSERT INTO Products (Name, Price, Details)
VALUES 
    ('Product A', 19.99, 'Details about Product A'),
    ('Product B', 29.99, 'Details about Product B'),
    ('Product C', 39.99, 'Details about Product C');



CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50),
    Email NVARCHAR(255) UNIQUE,
	Identification NVARCHAR(15) UNIQUE,
    Password NVARCHAR(255),
    Status BIT DEFAULT 1,
	Phone NVARCHAR(20),
	Role NVARCHAR(20)
);



CREATE TABLE Invoices (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    total_amount DECIMAL(10,2),
    creation_date DATETIME,
    print_date DATETIME,
    CONSTRAINT fk_user_invoices FOREIGN KEY (user_id) REFERENCES Users(id)
);


CREATE TABLE Invoice_Items (
    id INT PRIMARY KEY IDENTITY(1,1),
    invoice_id INT,
    product_id INT,
    quantity INT,
    price_excl_tax DECIMAL(10,2),
    CONSTRAINT fk_invoice_invoice_items FOREIGN KEY (invoice_id) REFERENCES Invoices(id),
    CONSTRAINT fk_product_invoice_items FOREIGN KEY (product_id) REFERENCES Products(id)
);
