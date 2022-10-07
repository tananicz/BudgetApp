using BudgetApp.Models;

namespace BudgetApp.Repository
{
    public interface IDataRepository
    {
        public List<Item>? GetItems(bool includeCategories, Func<Item, bool> filterAdapter);
        public Task<Item?> GetItem(int id);
        public Task AddItem(Item item);
        public Task UpdateItem(Item item);
        public Task RemoveItem(Item item);
        public List<Category>? GetCategories();
        public Task<Category?> GetCategory(int id);
        public Task AddCategory(Category category);
        public Task UpdateCategory(Category category);
        public Task RemoveCategory(Category category);
        public bool CheckEntityExistence(Type entityType, int key);
    }
}
