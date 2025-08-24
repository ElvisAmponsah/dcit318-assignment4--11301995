-- Create MedicalDB Database
CREATE DATABASE MedicalDB;
GO

USE MedicalDB;
GO

-- Create Doctors Table
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT DEFAULT 1
);

-- Create Patients Table
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL
);

-- Create Appointments Table
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT FOREIGN KEY REFERENCES Doctors(DoctorID),
    PatientID INT FOREIGN KEY REFERENCES Patients(PatientID),
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(500)
);

-- Insert Sample Data into Doctors
INSERT INTO Doctors (FullName, Specialty, Availability) VALUES
('Dr. John Smith', 'Cardiology', 1),
('Dr. Sarah Johnson', 'Pediatrics', 1),
('Dr. Michael Brown', 'Orthopedics', 1),
('Dr. Emily Davis', 'Neurology', 1),
('Dr. Robert Wilson', 'Dermatology', 1);

-- Insert Sample Data into Patients
INSERT INTO Patients (FullName, Email) VALUES
('Alice Johnson', 'alice.johnson@email.com'),
('Bob Smith', 'bob.smith@email.com'),
('Carol White', 'carol.white@email.com'),
('David Brown', 'david.brown@email.com'),
('Eva Garcia', 'eva.garcia@email.com');

-- Insert Sample Appointments
INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES
(1, 1, '2024-01-15 10:00:00', 'Regular checkup'),
(2, 3, '2024-01-16 14:30:00', 'Follow-up appointment'),
(3, 2, '2024-01-17 09:00:00', 'Consultation');
GO

