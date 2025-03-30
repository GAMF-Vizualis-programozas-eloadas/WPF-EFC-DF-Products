USE Products;

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(40),
    CategoryID INT,
    UnitPrice MONEY,
    UnitsInStock SMALLINT,
    Discontinued BIT
);

INSERT INTO Products.dbo.Products (ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued)
SELECT ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued
FROM [C:\MUNKA\WPF-EFC-DF-NORTHWIND\NORTHWIND.MDF].dbo.Products;