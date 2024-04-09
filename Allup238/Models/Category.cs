using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllUpMVC.Models
{
    public class Category : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        public List<Product>? Products { get; set; }
        public string? CategoryImage { get; set; }
        [NotMapped]
        public IFormFile? CategoryImageFile { get; set; }
    }
}
