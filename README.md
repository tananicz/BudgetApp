Simple Budget App. 

The goal of the application is to present a simple interface to store one's expenses and incomes. Records must have assigned a date and a category before storing them in the database, hence there's also a category manager to create, edit and delete categories. The user of an application is thus able to display his incomes/outcomes according to the category or some particular period in time.

My primary goal was to work out the solution mostly on the server side to show my grip on ASP.NET Core. I wanted an app to look like it was done on client-side framework. For example, when the user chooses a category and some particular period of time and suddenly decides to edit some of the filetered items, after saving changes he goes back to the records he previously filtered and sees all the chosen criteria (that's not the case however when the user creates a new record - default date is current one by default and after saving changes user goes to the first page of unfiltered data). The whole mechanism is achieved without using session and cookies.

The code itself is a mixed bag of things available in ASP.NET Core framework. It's built around MVC pattern, but not using the MVC template built in Visual Studio. I started with empty project, added single AddControllersWithViews service and configured routing middleware. In addition to controllers, views and partial views there's also single custom filter and custom tag helper included. The ItemsInCategory method in CategoriesController returns JSON instead of ViewResult to show that the action may be called from JavaScript code in order to ascertain whether there are any items in category before it can be deleted. In fact, JavaScript is used only in small doses, mostly for cosmetic purposes. 

Data verification is achieved by assigining attributes to model classes, which action results in html attributes automatically added to html response. That allows client side validation after including jQuery unobtrusive validation library, which I used. However, even if that library wasn't added, the code will perform validation on the server side using ModelState object and tag helpers built in ASP.NET Core. 

Requirements:
- .NET 6.0 SDK

Optional:
- Microsoft Entity Framework Tools package
- Some Database Server, i.e. MS SQL Server

In order to run an app please do the following:
- go to main directory (containing "BudgetApp.csproj" file) and run command: dotnet run --isMemoryDb=true

It's also possible to run app with SQL Server:
- go to main directory (containing "BudgetApp.csproj" file), open "appsettings.json" file and substitute connection string provided for "BudgetDbConnectionStr" to match your database (example for MS SQL: "Server=__SERVER_NAME__\\__DB_INSTANCE__;Database=__DB_NAME__;Trusted_Connection=True;MultipleActiveResultSets=True")
- in the same direcotry run command: dotnet ef database update
- in the same directory run command: dotnet run
