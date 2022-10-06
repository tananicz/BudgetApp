using BudgetApp.Models;
using BudgetApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BudgetDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:BudgetDbConnectionStr"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<MvcOptions>(opts => {
    opts.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((value) => "Proszê wprowadziæ wartoœæ");
    opts.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, type) => $"Wartoœæ '{value}' nie jest poprawna dla pola typu {type}");
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    DbSeeder.InitializeDb(scope.ServiceProvider.GetRequiredService<BudgetDbContext>(), app.Services.GetRequiredService<ILogger<AppHelper>>());
}
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    //4 args
    endpoints.MapControllerRoute("withCategoryPageFromDateToDate", "/zakresdat/{fromDate:datetime}/{toDate:datetime}/kategoria/{catId:int}/strona/{page:int}", new { controller = "Items", action = "List" });
    //3 args
    endpoints.MapControllerRoute("withCategoryPageFromDate", "/dataod/{fromDate:datetime}/kategoria/{catId:int}/strona/{page:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategoryPageToDate", "/datado/{toDate:datetime}/kategoria/{catId:int}/strona/{page:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withPageFromDateToDate", "/zakresdat/{fromDate:datetime}/{toDate:datetime}/strona/{page:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategoryFromDateToDate", "/zakresdat/{fromDate:datetime}/{toDate:datetime}/kategoria/{catId:int}", new { controller = "Items", action = "List" });
    //2 args
    endpoints.MapControllerRoute("withFromDateToDate", "/zakresdat/{fromDate:datetime}/{toDate:datetime}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategoryToDate", "/datado/{toDate:datetime}/kategoria/{catId:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategoryFromDate", "/dataod/{fromDate:datetime}/kategoria/{catId:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withPageToDate", "/datado/{toDate:datetime}/strona/{page:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withPageFromDate", "/dataod/{fromDate:datetime}/strona/{page:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategoryPage", "/kategoria/{catId:int}/strona/{page:int}", new { controller = "Items", action = "List" });
    //1 arg
    endpoints.MapControllerRoute("withFromDate", "/dataod/{fromDate:datetime}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withToDate", "/datado/{toDate:datetime}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withCategory", "/kategoria/{catId:int}", new { controller = "Items", action = "List" });
    endpoints.MapControllerRoute("withPage", "/strona/{page:int}", new { controller = "Items", action = "List" });
    //defaults
    endpoints.MapControllerRoute("edit", "/edycja/{id}", new { controller = "Items", action = "Edit" });
    endpoints.MapControllerRoute("new", "/nowapozycja", new { controller = "Items", action = "New" });
    endpoints.MapControllerRoute("save", "/zapisywanie", new { controller = "Items", action = "AddOrUpdateItem" });
    endpoints.MapControllerRoute("categories", "/kategorie", new { controller = "Categories", action = "Show" });
    endpoints.MapControllerRoute("modifyCategory", "/modyfikujkategorie", new { controller = "Categories", action = "ModifyCategory" });

    endpoints.MapControllerRoute("default", "/{controller=Items}/{action=List}/{id:int?}");
});
app.Run();