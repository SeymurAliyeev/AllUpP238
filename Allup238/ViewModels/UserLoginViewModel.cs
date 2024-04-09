using System.ComponentModel.DataAnnotations;

namespace AllUpMVC.ViewModels
{
    public class UserLoginViewModel
    {
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
