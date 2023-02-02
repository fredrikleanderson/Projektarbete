# Projektarbete

Setting up:

* Create two local database-files, one for Entity Framework and one for Dapper
* In ORMComparison: Replace the connection strings in appsettings.json with those of your databases
* In Data: Run update-database to set up your EntityFramework-database
* Run the script CreateTablesAndIndexes.sql on your Dapper-database
* Run the script CreateStoredProcedures.sql on your Dapper-database
* In ORMComparison: Customize your tests in Program.cs
* You're good to go
