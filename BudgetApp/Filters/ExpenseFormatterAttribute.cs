using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace BudgetApp.Filters
{
    public class ExpenseFormatterAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Dictionary<string, StringValues> formData = new Dictionary<string, StringValues>();
            foreach(var kvp in context.HttpContext.Request.Form)
                formData[kvp.Key] = kvp.Value;
            
            if (formData.ContainsKey("Expense"))
            {
                string expense = formData["Expense"].ToString();
                expense = expense.Replace('.', ',').Replace("zł", "").Replace(@"\s+", "").Trim();
                formData["Expense"] = expense;
            }

            context.HttpContext.Request.Form = new FormCollection(formData);
            await next();
        }
    }
}
