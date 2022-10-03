using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BudgetApp.TagHelpers
{
    [HtmlTargetElement("input", Attributes = "exclude-default-js-number-validation")]
    public class ExpenseTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            int index = output.Attributes.IndexOfName("data-val-number");
            output.Attributes.RemoveAt(index);
        }
    }
}
