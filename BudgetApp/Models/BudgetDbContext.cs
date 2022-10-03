using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> opts) : base(opts) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
