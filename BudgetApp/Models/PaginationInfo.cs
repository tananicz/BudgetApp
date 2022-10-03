namespace BudgetApp.Models
{
    public class PaginationInfo
    {
        public int TotalPages { get; set; }
        public int DefaultItemsPerPage { get; set; }
        public Dictionary<string, string> RouteParams { get; set; }
    }
}
