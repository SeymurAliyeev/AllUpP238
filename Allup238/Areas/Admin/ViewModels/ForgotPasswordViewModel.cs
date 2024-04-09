using System.ComponentModel.DataAnnotations;

namespace AllupP238.Areas.Admin.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
