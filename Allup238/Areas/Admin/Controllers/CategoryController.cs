//using AllupP238.Business.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace AllUpMVC.Areas.Admin.Controllers
//{
//    [Area("admin")]
//    public class CategoryController : Controller
//    {
//        private readonly ICategoryService _CategoryService;

//        public CategoryController(ICategoryService CategoryService)
//        {
//            _CategoryService = CategoryService;
//        }

//        public async Task<IActionResult> Index()
//            => View(await _CategoryService.GetAllAsync(x=>x.IsDeleted == false,"Products"));

//        public IActionResult Create()
//            => View();

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Models.Category Category)
//        {
//            if (!ModelState.IsValid) return View();

//            try
//            {
//                await _CategoryService.CreateAsync(Category);
//            }
//            catch (CustomExceptions.Common.NameAlreadyExistException ex)
//            {
//                ModelState.AddModelError(ex.PropertyName, ex.Message);
//                return View();
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", ex.Message);
//                return View();
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        public async Task<IActionResult> Update(int id)
//        {
//            Category Category = null;
//            try
//            {
//                Category = await _CategoryService.GetByIdAsync(id);
//            }
//            catch (CustomExceptions.CategoryExceptions.CategoryNotFoundException ex)
//            {
//                return View("Error");
//            }
//            catch (Exception)
//            {

//                throw;
//            }

//            return View(Category);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Update(Models.Category Category)
//        {
//            if (!ModelState.IsValid) return View();

//            try
//            {
//                await _CategoryService.UpdateAsync(Category);
//            }
//            catch (CustomExceptions.Common.NameAlreadyExistException ex)
//            {
//                ModelState.AddModelError(ex.PropertyName, ex.Message);
//                return View();
//            }
//            catch (CustomExceptions.CategoryExceptions.CategoryNotFoundException ex)
//            {
//                return View("Error");
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", ex.Message);
//                return View();
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                bool status = await _CategoryService.CheckChildAsync(id);
//                if (status)
//                {
//                    await _CategoryService.DeleteAsync(id);
//                }
//                else
//                {
//                    return BadRequest();
//                }
//            }
//            catch (CustomExceptions.CategoryExceptions.CategoryNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception)
//            {
//                return NotFound();
//            }
//            return Ok();
//        }
//    }
//}


using AllupP238.Data;
using AllupP238.Helpers.Extensions;
using AllupWebApplication.Business.Interfaces;
using AllupWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllupWebApplication.Controllers;
[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AllupDbContext _context;
    public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, AllupDbContext context)
    {
        _categoryService = categoryService;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    public async Task<IActionResult> Index(int? pageNumber)
    {
        const int pageSize = 10; // Set the number of items you want per page
        var query = _context.Categories.AsQueryable(); // Assuming _context is your DbContext and Categories is your DbSet

        // Include any related data if needed
        // query = query.Include(c => ...);

        var paginatedData = await PaginatedList<Category>.CreateAsync(query, pageNumber ?? 1, pageSize);
        return View(paginatedData);
    }

    public IActionResult Create()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.CreateCategoryAsync(category, imageFile);
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.UpdateCategoryAsync(category, imageFile);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }
        return View(category);
    }
    // GET: Shows confirmation page for soft delete
    [HttpGet]
    public async Task<IActionResult> SoftDeleteConfirm(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Performs the actual soft delete
    [HttpPost, ActionName("SoftDeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category != null)
        {
            await _categoryService.SoftDeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }
        return NotFound();
    }

    // GET: Shows confirmation page for hard delete
    [HttpGet]
    public async Task<IActionResult> HardDeleteConfirm(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Performs the actual hard delete
    [HttpPost, ActionName("HardDeleteConfirm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDeleteConfirmed(int id)
    {
        var categoryExists = await _categoryService.GetCategoryByIdAsync(id) != null;
        if (!categoryExists)
        {
            return NotFound();
        }
        await _categoryService.HardDeleteCategoryAsync(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}
