using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.Models
{
    public class ListItemsModel
    {
        public IEnumerable<Item> Items { get; set; }
        public SelectList CategoriesSelectList { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
