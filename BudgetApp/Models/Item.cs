using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić nazwę wydatku lub dochodu")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [Display(Name = "Kwota")]
        [Required(ErrorMessage = "Proszę podać wartość wydatku lub dochodu")]
        [RegularExpression(@"^(\s*\d){1,6}(\s*(\.|\,){1}(\s*\d){1,2})?\s*(zł)?\s*$", ErrorMessage = "Proszę podać niezerową wartość dodatnią mniejszą od 999 999,99 zł")]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public decimal Expense { get; set; }

        [Required(ErrorMessage = "Proszę podać poprawną datę")]
        public DateTime DateTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Proszę wybrać jakąś kategorię")]
        public int CategoryId { get; set; }

        [BindNever]
        public Category Category { get; set; }
    }
}
