using BudgetApp.Filters;
using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ItemsController : Controller
    {
        private const int defaultItemsPerPage = 10;
        private const string dateFormat = "yyyy-MM-dd";
        private BudgetDbContext dbContext;

        public ItemsController(BudgetDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IActionResult List(DateTime? fromDate, DateTime? toDate, int page = 1, int catId = 0, int itemsPerPage = defaultItemsPerPage)
        {
            int skip = (page - 1) * itemsPerPage;

            IEnumerable<Item> itemsFiltered = dbContext.Items.Include(i => i.Category).Where(AppHelper.FilterAdapter(catId, fromDate, toDate)).OrderByDescending(i => i.DateTime);
            Dictionary<string, string> routeParamsForModel = PrepareRouteParamsForModel(page, itemsPerPage, catId, fromDate, toDate);

            ListItemsModel model = new ListItemsModel {
                Items = itemsFiltered.Skip(skip).Take(itemsPerPage),
                CategoriesSelectList = PrepareSelectList(true, "Wszystkie kategorie"),
                PaginationInfo = new PaginationInfo {
                    TotalPages = (int)Math.Ceiling(itemsFiltered.Count() / (float)itemsPerPage),
                    DefaultItemsPerPage = defaultItemsPerPage,
                    RouteParams = routeParamsForModel,
                }
            };

            return View(model);
        }

        public IActionResult New()
        {
            ViewBag.CategoriesList = PrepareSelectList(true, "Wybierz kategorię");
            return View("CreateOrEdit", new Item());
        }

        public async Task<IActionResult> Edit(int id)
        {
            Item? item = await dbContext.Items.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
            if (null != item)
            {
                ViewBag.CategoriesList = PrepareSelectList();
                return View("CreateOrEdit", item);
            }
            else
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        [ExpenseFormatter]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItem(Item item, bool outcome, bool isNew)
        {
            if (isNew && item.Id != 0)
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            if (!isNew && !AppHelper.CheckEntityExistence(dbContext, typeof(Item), item.Id))
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            
            if (!AppHelper.CheckEntityExistence(dbContext, typeof(Category), item.CategoryId))
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            ModelState.Remove(nameof(Item.Category));

            if (ModelState.IsValid)
            {
                if (outcome)
                    item.Expense *= -1;

                if (isNew)
                    await dbContext.Items.AddAsync(item);
                else
                    dbContext.Items.Update(item);

                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List), isNew ? null : (((Dictionary<string, string>?) TempData["paramsDictionary"]) ?? null));
            }
            else
            {
                if (isNew)
                    ViewBag.CategoriesList = PrepareSelectList(true, "Wybierz kategorię");
                else
                    ViewBag.CategoriesList = PrepareSelectList();
                return View("CreateOrEdit", item);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(Item item)
        {
            if (!AppHelper.CheckEntityExistence(dbContext, typeof(Item), item.Id))
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            Dictionary<string, string>? paramsDictionary = (Dictionary<string, string>?) TempData["paramsDictionary"] ?? null;

            dbContext.Items.Remove(item);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(List), paramsDictionary);
        }

        private SelectList PrepareSelectList(bool addEmptyOption = false, string emptyOptionLabel = "")
        {
            IEnumerable<Category>? categories = null;

            if (addEmptyOption)
            {
                List<Category> catList = new List<Category>(dbContext.Categories);
                catList.Add(new Category
                {
                    CategoryId = 0,
                    Name = emptyOptionLabel
                });
                categories = catList.OrderBy(c => c.CategoryId);
            }
            else
                categories = dbContext.Categories;

            return new SelectList(categories, "CategoryId", "Name");
        }

        private Dictionary<string, string> PrepareRouteParamsForModel(int page, int itemsPerPage, int catId, DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            if (page != 1)
                output.Add("page", page.ToString());
            if (itemsPerPage != defaultItemsPerPage)
                output.Add("itemsPerPage", itemsPerPage.ToString());
            if (catId != 0)
                output.Add("catId", catId.ToString());
            if (fromDate.HasValue)
                output.Add("fromDate", fromDate.Value.ToString(dateFormat));
            if (toDate.HasValue)
                output.Add("toDate", toDate.Value.ToString(dateFormat));

            return output;
        }
    }
}
