using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kategorii")]
        [MaxLength(255)]
        public string Name { get; set; }

        [BindNever]
        public IEnumerable<Item> Items { get; set; }
    }
}
