using BudgetApp.Filters;
using BudgetApp.Models;
using BudgetApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ItemsController : Controller
    {
        private const int DefaultItemsPerPage = 10;
        private const string DateFormat = "yyyy-MM-dd";

        private readonly IDataRepository _dataRepository;

        public ItemsController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public IActionResult List(DateTime? fromDate, DateTime? toDate, int page = 1, int catId = 0, int itemsPerPage = DefaultItemsPerPage)
        {
            int skip = (page - 1) * itemsPerPage;

            List<Item>? itemsFiltered = _dataRepository.GetItems(true, AppHelper.FilterAdapter(catId, fromDate, toDate));
            Dictionary<string, string> routeParamsForModel = PrepareRouteParamsForModel(page, itemsPerPage, catId, fromDate, toDate);

            ListItemsModel model = new ListItemsModel {
                Items = (itemsFiltered != null) ? itemsFiltered.Skip(skip).Take(itemsPerPage) : new List<Item>(),
                CategoriesSelectList = PrepareSelectList(true, "Wszystkie kategorie"),
                PaginationInfo = new PaginationInfo {
                    TotalPages = (int)Math.Ceiling(((itemsFiltered != null) ? itemsFiltered.Count() : 0) / (float)itemsPerPage),
                    DefaultItemsPerPage = DefaultItemsPerPage,
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
            Item? item = await _dataRepository.GetItem(id);

            if (null != item)
            {
                ViewBag.CategoriesList = PrepareSelectList();
                return View("CreateOrEdit", item);
            }
            else
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        [ExpenseFormatter]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItem(Item item, bool outcome, bool isNew)
        {
            if (isNew && item.Id != 0) 
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            if (!isNew && !_dataRepository.CheckEntityExistence(typeof(Item), item.Id)) 
            {    
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            if (!_dataRepository.CheckEntityExistence(typeof(Category), item.CategoryId))
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            ModelState.Remove(nameof(Item.Category));

            if (ModelState.IsValid)
            {
                if (outcome)
                { 
                    item.Expense *= -1;
                }

                if (isNew)
                { 
                    await _dataRepository.AddItem(item);
                }
                else
                {
                    await _dataRepository.UpdateItem(item);
                }

                return RedirectToAction(nameof(List), isNew ? null : (((Dictionary<string, string>?) TempData["paramsDictionary"]) ?? null));
            }
            else
            {
                if (isNew)
                { 
                    ViewBag.CategoriesList = PrepareSelectList(true, "Wybierz kategorię");
                }
                else
                { 
                    ViewBag.CategoriesList = PrepareSelectList();
                }
                return View("CreateOrEdit", item);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(Item item)
        {
            if (!_dataRepository.CheckEntityExistence(typeof(Item), item.Id))
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            Dictionary<string, string>? paramsDictionary = (Dictionary<string, string>?) TempData["paramsDictionary"] ?? null;

            await _dataRepository.RemoveItem(item);

            return RedirectToAction(nameof(List), paramsDictionary);
        }

        private SelectList PrepareSelectList(bool addEmptyOption = false, string emptyOptionLabel = "")
        {
            IEnumerable<Category>? categories = _dataRepository.GetCategories();

            if (addEmptyOption && categories != null)
            {
                List<Category> catList = new List<Category>(categories);
                catList.Add(new Category
                {
                    CategoryId = 0,
                    Name = emptyOptionLabel
                });
                categories = catList.OrderBy(c => c.CategoryId);
            }

            return new SelectList(categories, "CategoryId", "Name");
        }

        private Dictionary<string, string> PrepareRouteParamsForModel(int page, int itemsPerPage, int catId, DateTime? fromDate, DateTime? toDate)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            if (page != 1)
                output.Add("page", page.ToString());
            if (itemsPerPage != DefaultItemsPerPage)
                output.Add("itemsPerPage", itemsPerPage.ToString());
            if (catId != 0)
                output.Add("catId", catId.ToString());
            if (fromDate.HasValue)
                output.Add("fromDate", fromDate.Value.ToString(DateFormat));
            if (toDate.HasValue)
                output.Add("toDate", toDate.Value.ToString(DateFormat));

            return output;
        }
    }
}
