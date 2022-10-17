using BudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Repository
{
    public class BudgetAppDataRepository : IDataRepository
    {
        private BudgetDbContext _dbContext;

        public BudgetAppDataRepository(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Item>? GetItems(bool includeCategories, Func<Item, bool> filterAdapter) 
        {
            if (includeCategories)
            {
                return _dbContext
                    .Items?
                    .Include(i => i.Category)
                    .Where(filterAdapter)
                    .OrderByDescending(i => i.DateTime)
                    .ToList();
            }
            else
            {
                return _dbContext
                    .Items?
                    .Where(filterAdapter)
                    .OrderByDescending(i => i.DateTime)
                    .ToList();
            }
        }

        public async Task<Item?> GetItem(int id)
        {
            if (_dbContext.Items == null)
            { 
                return null;
            }
            else
            { 
                return await _dbContext
                    .Items
                    .Include(i => i.Category)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
        }

        public async Task AddItem(Item item)
        {
            if (_dbContext.Items != null)
            {
                await _dbContext.Items.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddItems(params Item[] items)
        {
            if (_dbContext.Items != null)
            {
                await _dbContext.Items.AddRangeAsync(items);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateItem(Item item)
        {
            if (_dbContext.Items != null)
            {
                _dbContext.Items.Update(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveItem(Item item)
        {
            if (_dbContext.Items != null)
            {
                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        public List<Category>? GetCategories()
        {
            return _dbContext.Categories?.ToList();
        }

        public async Task<Category?> GetCategory(int id)
        {
            if (_dbContext.Categories == null)
            {
                return null;
            }
            else
            {
                return await _dbContext
                    .Categories
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);
            }
        }

        public async Task AddCategory(Category category)
        {
            if (_dbContext.Categories != null)
            {
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateCategory(Category category)
        {
            if (_dbContext.Categories != null)
            {
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveCategory(Category category)
        {
            if (_dbContext.Categories != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public bool CheckEntityExistence(Type entityType, int key)
        {
            object? entity = _dbContext.Find(entityType, key);
            if (entity == null)
            {
                return false;
            }
            else
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
                return true;
            }
        }

        public int CountEntities<T>() where T : class
        {
            return _dbContext.Set<T>().Count();
        }
    }
}
