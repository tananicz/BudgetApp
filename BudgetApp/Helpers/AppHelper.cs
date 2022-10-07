using BudgetApp.Models;

namespace BudgetApp.Helpers
{
    public static class AppHelper
    {
        public static Dictionary<string, string> ResetAmbientRouteValues() => new Dictionary<string, string>()
        {
            { "page", "" },
            { "catId", "" },
            { "fromDate", "" },
            { "toDate", "" },
            { "itemsPerPage", "" },
        };

        public static Func<Item, bool> FilterAdapter(int catId, DateTime? fromDate, DateTime? toDate)
        {
            return (item) =>
            {
                bool result = false;

                if (fromDate.HasValue && toDate.HasValue)
                { 
                    result = DateTime.Compare(fromDate.Value, item.DateTime) <= 0 && DateTime.Compare(item.DateTime, toDate.Value) <= 0;
                }
                else if (fromDate.HasValue)
                { 
                    result = DateTime.Compare(fromDate.Value, item.DateTime) <= 0;
                }
                else if (toDate.HasValue)
                { 
                    result = DateTime.Compare(item.DateTime, toDate.Value) <= 0;
                }
                else
                { 
                    result = true;
                }

                result = result && (catId == 0 || item.CategoryId == catId);

                return result;
            };
        }

        public static void AddUserFriendlyRoutes(this IEndpointRouteBuilder endpoints)
        {
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
        }
    }
}
