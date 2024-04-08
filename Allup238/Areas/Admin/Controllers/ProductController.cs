using AllupP238.Business.Interfaces;
using AllupP238.Data;
using AllupWebApplication.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(AllupDbContext context, 
                            IWebHostEnvironment env,
                            IProductService productService,
                            ICategoryService categoryService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
            => View(await _productService.GetAllAsync(null,"Author","Genre","BookImages"));

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _context.Categories.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Product product)
        {
            ViewBag.Genres = await _categoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            try
            {
                await _productService.CreateAsync(product);
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

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Genres = await _categoryService.GetAllAsync();
            Models.Product? product = null;
            try
            {
                product = await _productService.GetSingleAsync(x=>x.Id == id,"Category","ProductImages");
            }
            catch (Exception)
            {

                throw;
            }

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            ViewBag.Genres = await _categoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            Product existData = await _productService.GetSingleAsync(x => x.Id == product.Id, "Category", "ProductImages");

            if(product.PosterImageFile is not null)
            {
                if (product.PosterImageFile.ContentType != "image/jpeg" && product.PosterImageFile.ContentType != "image/png")
                {
                    throw new ProductInvalidCredentialException("PosterImageFile", "Content type must be png or jpeg!");
                }

                if (product.PosterImageFile.Length > 2097152)
                {
                    throw new ProductInvalidCredentialException("PosterImageFile", "Size must be lower than 2mb!");
                }
                FileManager.DeleteFile(_env.WebRootPath, "uploads/books", existData.ProductImages.FirstOrDefault(x => x.IsPoster == true)?.ImageUrl);
                if(existData.ProductImages.Any(x=>x.IsPoster == true))
                {
                    existData.ProductImages.RemoveAll(x => x.IsPoster == true);
                }
                ProductImage posterImage = new ProductImage()
                {
                    Book = existData,
                    ImageUrl = product.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/books"),
                    IsPoster = true
                };
                await _context.ProductImages.AddAsync(posterImage);
            }
            if(product.HoverImageFile is not null)
            {
                if (product.HoverImageFile.ContentType != "image/jpeg" && product.HoverImageFile.ContentType != "image/png")
                {
                    throw new ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                }

                if (product.HoverImageFile.Length > 2097152)
                {
                    throw new ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                }
                ProductImage hoverImage = new ProductImage()
                {
                    Book = existData,
                    ImageUrl = product.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/books"),
                    IsPoster = false
                };
                await _context.ProductImages.AddAsync(hoverImage);
            }

            foreach (var imageFile in existData.BookImages.Where(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsPoster == null))
            {
                FileManager.DeleteFile(_env.WebRootPath, "uploads/books", imageFile?.ImageUrl);
            }
            existData.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsPoster == null);

            if(product.ImageFiles is not null)
            {
                foreach (var imageFile in product.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                    }

                    if (imageFile.Length > 2097152)
                    {
                        throw new ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                    }

                    ProductImage bookImage = new ProductImage()
                    {
                        ProductId = product.Id,
                        IsPoster = null,
                        ImageUrl = imageFile.SaveFile(_env.WebRootPath, "uploads/books")
                    };

                    existData.ProductImages.Add(bookImage);
                }
            }
            existData.Desc = product.Desc;
            existData.Title = product.Title;
            existData.IsNew = product.IsNew;
            existData.CategoryId = product.CategoryId;
            existData.ProductCode = product.ProductCode;
            existData.CostPrice = product.CostPrice;
            existData.SalePrice = product.SalePrice;
            existData.IsDeleted = product.IsDeleted;
            existData.IsBestSeller = product.IsBestSeller;
            existData.DiscountPercent = product.DiscountPercent;
            existData.ModifiedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
