using AllUpMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace AllUpMVC.ViewServices
{
    public class UserLayoutService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserLayoutService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<AppUser>GetUserName()
        {
            return null;
        }
    }
}
