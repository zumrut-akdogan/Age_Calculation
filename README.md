Requirements & Setup

To run this project, please follow the steps below:

🧩 Prerequisites
Install Visual Studio 2022
Install SQL Server Management Studio (SSMS)
🗄️ Database Setup
Import the provided .bak database file into SQL Server.

Restore the database with the name:

AgeCalculationDb
🔗 Connection String Configuration
Navigate to the Context folder in the project.
Open the AgeCalculateDbContext class.
Update the SQL connection string with your own server name.

Example connection string:

Data Source=(ServerName);Initial Catalog=AgeCalculationDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

👉 Replace (ServerName) with your actual SQL Server name.

▶️ Run the Project
Build the project in Visual Studio
Run the application
