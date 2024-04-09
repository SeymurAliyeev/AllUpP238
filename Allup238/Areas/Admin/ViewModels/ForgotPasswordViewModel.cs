using System.ComponentModel.DataAnnotations;

namespace AllUpMVC.Areas.Admin.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
