using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public static class AppHelper
    {
        public static bool CheckEntityExistence(DbContext dbContext, Type entityType, int key)
        {
            object? entity = dbContext.Find(entityType, key);
            if (entity == null)
                return false;
            else
            {
                dbContext.Entry(entity).State = EntityState.Detached;
                return true;
            }
        }

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
            return ((Item item) => {
                bool result = false;

                if ((fromDate.HasValue) && (toDate.HasValue))
                    result = (DateTime.Compare(fromDate.Value, item.DateTime) <= 0) && (DateTime.Compare(item.DateTime, toDate.Value) <= 0);
                else if (fromDate.HasValue)
                    result = (DateTime.Compare(fromDate.Value, item.DateTime) <= 0);
                else if (toDate.HasValue)
                    result = (DateTime.Compare(item.DateTime, toDate.Value) <= 0);
                else
                    result = true;

                result = result && ((catId == 0) || (item.CategoryId == catId));

                return result;
            });
        }
    }
}
