using BudgetApp.Models;
using BudgetApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CategoriesController : Controller
    {
        private readonly IDataRepository _dataRepository;

        public CategoriesController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public IActionResult Show()
        {
            return View(_dataRepository.GetCategories());
        }

        [HttpPost]
        public async Task<IActionResult> ModifyCategory(Category category, bool isNew)
        {
            if (isNew && category.CategoryId != 0)
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            if (!isNew && !_dataRepository.CheckEntityExistence(typeof(Category), category.CategoryId))
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            ModelState.Remove(nameof(Category.Items));

            if (ModelState.IsValid)
            {
                if (isNew)
                {
                    await _dataRepository.AddCategory(category);
                }
                else
                { 
                    await _dataRepository.UpdateCategory(category);
                }

                return RedirectToAction(nameof(Show));
            }
            else
            {
                return View("Error", category);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Category category)
        {
            ModelState.Remove(nameof(Category.Name));
            ModelState.Remove(nameof(Category.Items));

            if (_dataRepository.CheckEntityExistence(typeof(Category), category.CategoryId))
            {
                Category? catToRemove = await _dataRepository.GetCategory(category.CategoryId);

                if (catToRemove!.Items.Any())
                { 
                    ModelState.AddModelError("", "Items exist assigned to category");
                }

                if (ModelState.IsValid)
                {
                    await _dataRepository.RemoveCategory(catToRemove);
                    return RedirectToAction(nameof(Show));
                }
                else
                { 
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
            }
            else
            { 
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        public async Task<IActionResult> ItemsInCategory([FromRoute(Name = "id")] int categoryId)
        {
            Category? category = await _dataRepository.GetCategory(categoryId);
            int itemsCount = (category != null) ? category.Items.Count() : 0;
            return Json(new { count = itemsCount });
        }
    }
}
