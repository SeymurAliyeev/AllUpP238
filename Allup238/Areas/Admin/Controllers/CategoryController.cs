using Microsoft.AspNetCore.Mvc;
using AllupP238.Business.Interfaces;
using AllupP238.CustomExceptions.Common;
using AllupP238.CustomExceptions.CategoryExceptions;
using AllupP238.Models;
using AllupP238.Business.Interfaces;

namespace AllUpMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        public async Task<IActionResult> Index()
            => View(await _CategoryService.GetAllAsync(x=>x.IsDeleted == false,"Products"));

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category Category)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _CategoryService.CreateAsync(Category);
            }
            catch (NameAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Category Category = null;
            try
            {
                Category = await _CategoryService.GetByIdAsync(id);
            }
            catch (CategoryNotFoundException ex)
            {
                return View("Error");
            }
            catch (Exception)
            {

                throw;
            }

            return View(Category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category Category)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _CategoryService.UpdateAsync(Category);
            }
            catch (NameAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (CategoryNotFoundException ex)
            {
                return View("Error");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool status=await _CategoryService.CheckChildAsync(id);
                if (status)
                {
                    await _CategoryService.DeleteAsync(id);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (CategoryNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
