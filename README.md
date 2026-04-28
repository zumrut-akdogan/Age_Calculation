Age Calculation & Statistical Analysis Web App

📌 About the Project
This project is a web application that allows users to calculate their age based on their date of birth and perform statistical analysis on user data.
It is developed using ASP.NET Core MVC, Entity Framework Core, and SQL Server, providing a simple and user-friendly interface for data entry, visualization, and analysis.

🚀 Features
Age calculation based on date of birth
User data entry (name, surname, birth date, optional photo)
Client-side validation for form inputs
User listing with search and sorting options
 Statistical analysis with charts:
Gender distribution (Pie Chart)
Age group averages (Bar Chart: 0–15, 15–30, 30–45, 45+)
Fast and simple user experience

🛠️ Technologies Used
C#
ASP.NET Core MVC
Entity Framework Core
SQL Server
HTML / CSS / JavaScript
Chart.js
PagedList (Pagination)

⚙️ Installation & Setup
🧩 Prerequisites
Install Visual Studio 2022
Install SQL Server Management Studio (SSMS)

🗄️ Database Setup
Import the provided .bak file into SQL Server

Restore the database as:
AgeCalculationDb

🔗 Connection String Configuration
Go to the Context folder
Open AgeCalculateDbContext
Update the connection string with your SQL Server name

Example:
Data Source=(ServerName);Initial Catalog=AgeCalculationDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

👉 Replace (ServerName) with your own SQL Server name.

🎯 Purpose
The goal of this project is to:
Provide an easy way to calculate age
Deliver meaningful insights through statistical analysis
Demonstrate modern web development practices using ASP.NET Core

👤 Developer
Zümrüt Akdoğan
