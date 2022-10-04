using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CategoriesController : Controller
    {
        private BudgetDbContext dbContext;

        public CategoriesController(BudgetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Show()
        {
            return View(dbContext.Categories);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyCategory(Category category, bool isNew)
        {
            if (isNew && category.CategoryId != 0)
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            if (!isNew && !AppHelper.CheckEntityExistence(dbContext, typeof(Category), category.CategoryId))
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            ModelState.Remove(nameof(Category.Items));

            if (ModelState.IsValid)
            {
                if (isNew)
                    await dbContext.Categories.AddAsync(category);
                else
                    dbContext.Categories.Update(category);
                await dbContext.SaveChangesAsync();
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

            Category? catToRemove = await dbContext.Categories.Include(c => c.Items).Where(c => c.CategoryId == category.CategoryId).FirstOrDefaultAsync();
            if (catToRemove != null)
            {
                if (catToRemove.Items.Count() > 0)
                    ModelState.AddModelError("", "Items exist assigned to category");

                if (ModelState.IsValid)
                {
                    dbContext.Categories.Remove(catToRemove!);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Show));
                }
                else
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            else
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        public async Task<IActionResult> ItemsInCategory([FromRoute(Name = "id")] int categoryId)
        {
            Category? category = await dbContext.Categories.Include(c => c.Items).FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            int itemsCount = (category != null) ? category.Items.Count() : 0;
            return Json(new { count = itemsCount });
        }
    }
}
