CREATE DATABASE PharmacyDB;
GO

USE PharmacyDB;
GO

CREATE TABLE Medicines (
    MedicineID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Category VARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL
);

CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    MedicineID INT FOREIGN KEY REFERENCES Medicines(MedicineID),
    QuantitySold INT NOT NULL,
    SaleDate DATETIME NOT NULL
);

GO

CREATE PROCEDURE AddMedicine
    @Name VARCHAR(100),
    @Category VARCHAR(100),
    @Price DECIMAL(10,2),
    @Quantity INT
AS
BEGIN
    INSERT INTO Medicines (Name, Category, Price, Quantity)
    VALUES (@Name, @Category, @Price, @Quantity)
END
GO

CREATE PROCEDURE SearchMedicine
    @SearchTerm VARCHAR(100)
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    WHERE Name LIKE '%' + @SearchTerm + '%' OR Category LIKE '%' + @SearchTerm + '%'
END
GO

CREATE PROCEDURE UpdateStock
    @MedicineID INT,
    @Quantity INT
AS
BEGIN
    UPDATE Medicines
    SET Quantity = @Quantity
    WHERE MedicineID = @MedicineID
END
GO

CREATE PROCEDURE RecordSale
    @MedicineID INT,
    @QuantitySold INT
AS
BEGIN
    BEGIN TRANSACTION
    
    INSERT INTO Sales (MedicineID, QuantitySold, SaleDate)
    VALUES (@MedicineID, @QuantitySold, GETDATE())
    
    UPDATE Medicines
    SET Quantity = Quantity - @QuantitySold
    WHERE MedicineID = @MedicineID
    
    COMMIT TRANSACTION
END
GO

CREATE PROCEDURE GetAllMedicines
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    ORDER BY Name
END
GO

INSERT INTO Medicines (Name, Category, Price, Quantity) VALUES
('Aspirin', 'Pain Relief', 5.99, 100),
('Ibuprofen', 'Pain Relief', 7.99, 75),
('Amoxicillin', 'Antibiotics', 15.99, 50),
('Omeprazole', 'Digestive Health', 12.99, 60),
('Cetirizine', 'Allergy Relief', 8.99, 80);
GO

