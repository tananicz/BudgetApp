using BudgetApp.Helpers;
using BudgetApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BudgetDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:BudgetDbConnectionStr"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddScoped<IDataRepository, BudgetAppDataRepository>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<MvcOptions>(opts => {
    opts.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((value) => "Proszê wprowadziæ wartoœæ");
    opts.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, type) => $"Wartoœæ '{value}' nie jest poprawna dla pola typu {type}");
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    DbSeeder.InitializeDb(scope.ServiceProvider.GetRequiredService<IDataRepository>(), app.Services.GetRequiredService<ILogger<DbSeeder>>()).Wait();
}
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.AddUserFriendlyRoutes(); //comment this line if you want params passed in query string
    endpoints.MapControllerRoute("default", "/{controller=Items}/{action=List}/{id:int?}");
});
app.Run();