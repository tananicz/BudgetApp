Simple Budget App made using ASP.NET MVC and a little bit JS

Requirements:
- .NET 6.0 SDK
- Microsoft Entity Framework Tools package
- Some Database Server, i.e. MS SQL Server

In order to run an app please do the following:
- go to main directory (containing "BudgetApp.csproj" file), open "appsettings.json" file and substitute connection string provided for "BudgetDbConnectionStr" to match your database (example for MS SQL: "Server=__SERVER_NAME__\\__DB_INSTANCE__;Database=__DB_NAME__;Trusted_Connection=True;MultipleActiveResultSets=True")
- in the same direcotry run command: dotnet ef database update
- in the same directory run command: dotnet run
