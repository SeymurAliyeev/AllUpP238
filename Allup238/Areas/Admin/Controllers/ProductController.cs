using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AllUpP238.Business.Interfaces;
using AllUpP238.CustomExceptions.ProductExceptions;
using AllUpP238.Data;
using AllUpP238.Extensions;
using AllUpP238.Models;
using AllUpP238.Business.Implementations;
using AllupP238.Data;
using AllupP238.Business.Interfaces;
using AllupP238.Models;

namespace AllUpMVC.Areas.Admin.Controllers
{
    [Area("admin")]
   // [Authorize]
    public class ProductController : Controller
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _ProductService;
        private readonly ICategoryService _CategoryService;

        public ProductController(AllupDbContext context, 
                            IWebHostEnvironment env, 
                            IProductService ProductService, 
                            ICategoryService CategoryService)
        {
            _context = context;
            _env = env;
            _ProductService = ProductService;
            _CategoryService = CategoryService;
        }
        public async Task<IActionResult> Index()
            => View(await _ProductService.GetAllAsync(null,"Category","ProductImages"));

        public async Task<IActionResult> Create()
        {
            ViewBag.Categorys = await _context.Categories.ToListAsync();

            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categorys = await _CategoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            try
            {
                await _ProductService.CreateAsync(Product);
            }
            catch(ProductInvalidCredentialException ex)
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ProductService.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categorys = await _CategoryService.GetAllAsync();
            Product? Product = null;
            try
            {
                Product = await _ProductService.GetSingleAsync(x=>x.Id == id,"Category","ProductImages");
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
            ViewBag.Categorys = await _CategoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            Product existData = await _ProductService.GetSingleAsync(x => x.Id == Product.Id,  "Category", "ProductImages");

            if (Product.PosterImageFile is not null)
            {
                if (Product.PosterImageFile.ContentType != "image/jpeg" && Product.PosterImageFile.ContentType != "image/png")
                {
                    throw new ProductInvalidCredentialException("PosterImageFile", "Content type must be png or jpeg!");
                }

                if (Product.PosterImageFile.Length > 2097152)
                {
                    throw new ProductInvalidCredentialException("PosterImageFile", "Size must be lower than 2mb!");
                }
                FileManager.DeleteFile(_env.WebRootPath, "uploads/Products", existData.ProductImages.FirstOrDefault(x => x.IsPoster == true)?.ImageUrl);
                if(existData.ProductImages.Any(x=>x.IsPoster == true))
                {
                    existData.ProductImages.RemoveAll(x => x.IsPoster == true);
                }
                ProductImage posterImage = new ProductImage()
                {
                    Product = existData,
                    ImageUrl = Product.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/Products"),
                    IsPoster = true
                };
                await _context.ProductImages.AddAsync(posterImage);
            }
            if(Product.HoverImageFile is not null)
            {
                if (Product.HoverImageFile.ContentType != "image/jpeg" && Product.HoverImageFile.ContentType != "image/png")
                {
                    throw new ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                }

                if (Product.HoverImageFile.Length > 2097152)
                {
                    throw new ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                }
                ProductImage hoverImage = new ProductImage()
                {
                    Product = existData,
                    ImageUrl = Product.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/Products"),
                    IsPoster = false
                };
                await _context.ProductImages.AddAsync(hoverImage);
            }

            foreach (var imageFile in existData.ProductImages.Where(bi => !Product.ProductImageIds.Contains(bi.Id) && bi.IsPoster == null))
            {
                FileManager.DeleteFile(_env.WebRootPath, "uploads/Products", imageFile?.ImageUrl);
            }
            existData.ProductImages.RemoveAll(bi => !Product.ProductImageIds.Contains(bi.Id) && bi.IsPoster == null);

            if(Product.ImageFiles is not null)
            {
                foreach (var imageFile in Product.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                    }

                    if (imageFile.Length > 2097152)
                    {
                        throw new ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                    }

                    ProductImage ProductImage = new ProductImage()
                    {
                        ProductId = Product.Id,
                        IsPoster = null,
                        ImageUrl = imageFile.SaveFile(_env.WebRootPath, "uploads/Products")
                    };

                    existData.ProductImages.Add(ProductImage);
                }
            }
            existData.Desc = Product.Desc;
            existData.Title = Product.Title;
            existData.IsNew = Product.IsNew;
            existData.CategoryId = Product.CategoryId;
            existData.ProductCode = Product.ProductCode;
            existData.CostPrice = Product.CostPrice;
            existData.SalePrice = Product.SalePrice;
            existData.IsDeleted = Product.IsDeleted;
            existData.DiscountPercent = Product.DiscountPercent;
            existData.ModifiedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
