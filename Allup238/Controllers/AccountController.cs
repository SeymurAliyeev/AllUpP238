using AllUpMVC.Data;
using AllUpMVC.Models;
using AllUpMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllUpMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AllUpDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, AllUpDbContext context, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;

            user = await _userManager.FindByNameAsync(userVM.Username);

            if (user is null)
            {
                ModelState.AddModelError("", "istifadeci tapilmadi!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Melumatlar duzgun deyil!");
                return View();
            }

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelOnly", "Melumatlar duzgun yazilmayib");

                return View(model);
            } 
            AppUser member = new AppUser()
            {
                Fullname = model.Fullname,
                UserName = model.Username,
                Email = model.Email,

            };
            if (_context.Users.Any(x => x.NormalizedUserName == model.Username.ToUpper()))
            {
                ModelState.AddModelError("UserName", "Username is already exist!");
                return View();
            }
            if (_context.Users.Any(x => x.NormalizedEmail == model.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email is already exist!");
                return View();
            }

            var result = await _userManager.CreateAsync(member, model.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("ModelOnly", "Melumatlar duzgun yazilmayib");
                    return View();
                }
            }

            var roleResult = await _userManager.AddToRoleAsync(member, "Member");

            return RedirectToAction("login");
        }


    }
}
