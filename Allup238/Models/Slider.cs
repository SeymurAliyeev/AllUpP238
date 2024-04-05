using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AllupP238.Models
{
    public class Slider: BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }

        [StringLength(250)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]

        public IFormFile? ImageFile { get; set; }

        [StringLength(40)]
        public string? ButtonText { get; set; }

        [StringLength(250)]
        public string? ButtonUrl { get; set; }
    }
}
