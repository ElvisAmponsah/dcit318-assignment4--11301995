DCIT 318 Assignment 4- 11301995

 I made two Windows Forms applications that connect to SQL Server databases.

What I Made

1. Medical Appointment System
This lets patients book doctor appointments.

- Main form with buttons to go to different parts
- View doctors list
- Book new appointments
- Change or cancel appointments

2. Pharmacy System  
This helps manage medicine inventory and sales.

- Add new medicines
- Search medicines
- Update stock numbers
- Record when medicines are sold

How to Run

First, set up the databases:
1. Open SQL Server Management Studio
2. Run the DatabaseSetup.sql files for each project
3. This creates the databases with sample data

Then run the programs:
1. Open the .csproj files in Visual Studio
2. Build the projects (F6)
3. Run them (F5)

What I Used

- Windows Forms for the user interface
- ADO.NET to connect to SQL Server
- SqlConnection, SqlCommand, SqlDataReader
- DataGridView to show data
- Stored procedures for the pharmacy system
- Try-catch blocks for error handling

Files

Each project has:
- .cs files with the code
- .Designer.cs files for the forms
- .csproj project file
- App.config with database connection
- DatabaseSetup.sql to create the database

Screenshots

I took pictures of:
- Main forms working
- Data showing in grids
- Adding new records
- Searching and updating

This assignment shows I can use ADO.NET, Windows Forms, and SQL Server together to make working applications.
