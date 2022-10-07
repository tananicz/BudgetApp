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
    }
}
