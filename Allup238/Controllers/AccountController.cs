using AllupP238.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllUpMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly AllupDbContext _context;
        private readonly SignInManager<Models.AppUser> _signInManager;

        public AccountController(UserManager<Models.AppUser> userManager, AllupDbContext context, SignInManager<Models.AppUser> signInManager)
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
        public async Task<IActionResult> Login(ViewModels.UserLoginViewModel userVM)
        {
            if (!ModelState.IsValid) return View();
            Models.AppUser user = null;

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
        public async Task<IActionResult> Register(ViewModels.UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelOnly", "Melumatlar duzgun yazilmayib");

                return View(model);
            }
            Models.AppUser member = new Models.AppUser()
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
