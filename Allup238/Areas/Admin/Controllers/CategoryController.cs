using Microsoft.AspNetCore.Mvc;
using AllupP238.Business.Interfaces;
using AllupP238.CustomExceptions.Common;
using AllupP238.CustomExceptions.ProductExceptions;
using AllupP238.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly IProductService _ProductService;

        public CategoryController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        public async Task<IActionResult> Index()
            => View(await _ProductService.GetAllAsync(x=>x.IsDeleted == false && x.Name == "Sci-Fi","Books"));

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Product Product)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _ProductService.CreateAsync(Product);
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
            Product Product = null;
            try
            {
                Product = await _ProductService.GetByIdAsync(id);
            }
            catch (ProductNotFoundException ex)
            {
                return View("Error");
            }
            catch (Exception)
            {

                throw;
            }

            return View(Product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product Product)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _ProductService.UpdateAsync(Product);
            }
            catch (NameAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ProductNotFoundException ex)
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
                await _ProductService.DeleteAsync(id);
            }
            catch (ProductNotFoundException)
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
