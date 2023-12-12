# RoverPartners
RoverPartners is a data management app. It allows the school district to store, view, and edit information about business and community partners. Administrators have the ability to add new data, view the data, edit the data, and delete the data. 

**App features**: 
-Admin Navigation Bar
-CRUD Users
-CRUD Roles
-CRUD Partners
-CRUD Partner Types
-Light/Dark modes

## Prerequisites
-[Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with ASP.NET and web development workload
-[Latest .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-Install the latest .NET & EF CLI Tools by using this command :

    ```.NET Core CLI
    dotnet tool install --global dotnet-ef
    ```
-[GitHub Desktop](https://desktop.github.com/)
-[Axosoft's GitKraken](https://www.gitkraken.com/)

## Setup
To install the app:

-Download the .zip file from Github
-Open the .sln file

The project should open in Visual Studio. 

## Run the app!

-Open the solution in Visual Studio as stated in the Setup section
- Initialize the SQL Server Express LocalDB
  - In Visual Studio, go to `View > Other Windows > Package Manager Console`
  - In the console that appears at the bottom, type the command `Update-Database` and wait for the migration to finish.
-Run the project by clicking on the Debug drop down at the top of Visual Studio and select either "Start Debugging" or "Start Without Debugging"
  -"Start Without Debugging" is the faster option
- Seed Data
  - When running the project for the first time, the database will be seeded with an admin user.
  - You can log in to this account with the username `admin` and the password `Password123!`. It is _highly_ recommended that you change this password after logging in for the first time.

## Closing
Built for 2024 FBLA Coding & Programming competition. Built by Easton Area High School student Samir Issa, class of 2026
