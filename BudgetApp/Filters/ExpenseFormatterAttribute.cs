using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BudgetApp.Filters
{
    public class ExpenseFormatterAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Dictionary<string, StringValues> formData = new Dictionary<string, StringValues>();

            foreach(var kvp in context.HttpContext.Request.Form)
            { 
                formData[kvp.Key] = kvp.Value;
            }

            if (formData.ContainsKey("Expense"))
            {
                string expense = formData["Expense"].ToString();
                expense = Regex.Replace(expense, @"\s+", "");
                expense = expense.Replace(",", ".").Replace("zł", "");
                expense = decimal.Parse(expense, CultureInfo.InvariantCulture).ToString();
                formData["Expense"] = expense;
                context.HttpContext.Request.Form = new FormCollection(formData);
            }

            await next();
        }
    }
}
