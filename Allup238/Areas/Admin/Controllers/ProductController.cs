using AllUpMVC.Extensions;
using AllupP238.Business.Interfaces;
using AllupP238.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUpMVC.Areas.Admin.Controllers
{
    [Area("admin")]
   // [Authorize]
    public class ProductController : Controller
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _ProductService;
        private readonly CategoryService _CategoryService;

        public ProductController(AllupDbContext context, 
                            IWebHostEnvironment env, 
                            IProductService ProductService, 
                            CategoryService CategoryService)
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
        public async Task<IActionResult> Create(Models.Product product)
        {
            ViewBag.Categorys = await _CategoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            try
            {
                await _ProductService.CreateAsync(Product);
            }
            catch(CustomExceptions.ProductExceptions.ProductInvalidCredentialException ex)
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
        public async Task<IActionResult> Update(Models.Product Product)
        {
            ViewBag.Categorys = await _CategoryService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            Product existData = await _ProductService.GetSingleAsync(x => x.Id == Product.Id,  "Category", "ProductImages");

            if (Product.PosterImageFile is not null)
            {
                if (Product.PosterImageFile.ContentType != "image/jpeg" && Product.PosterImageFile.ContentType != "image/png")
                {
                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("PosterImageFile", "Content type must be png or jpeg!");
                }

                if (Product.PosterImageFile.Length > 2097152)
                {
                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("PosterImageFile", "Size must be lower than 2mb!");
                }
                Extensions.FileManager.DeleteFile(_env.WebRootPath, "uploads/Products", existData.ProductImages.FirstOrDefault(x => x.IsPoster == true)?.ImageUrl);
                if(existData.ProductImages.Any(x=>x.IsPoster == true))
                {
                    existData.ProductImages.RemoveAll(x => x.IsPoster == true);
                }
                Models.ProductImage posterImage = new Models.ProductImage()
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
                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                }

                if (Product.HoverImageFile.Length > 2097152)
                {
                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                }
                Models.ProductImage hoverImage = new Models.ProductImage()
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
                        throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("HoverImageFile", "Content type must be png or jpeg!");
                    }

                    if (imageFile.Length > 2097152)
                    {
                        throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("HoverImageFile", "Size must be lower than 2mb!");
                    }

                    Models.ProductImage productImage = new Models.ProductImage()
                    {
                        ProductId = Product.Id,
                        IsPoster = null,
                        ImageUrl = imageFile.SaveFile(_env.WebRootPath, "uploads/Products")
                    };

                    existData.ProductImages.Add(productImage);
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
