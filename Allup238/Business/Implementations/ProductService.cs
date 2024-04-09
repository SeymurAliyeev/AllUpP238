using AllUpMVC.Extensions;
using AllUpMVC.Models;
using AllupP238.Business.Interfaces;
using AllupP238.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AllUpMVC.Business.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AllupDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task CreateAsync(Models.Product Product)
        {
            if (Product.PosterImageFile.ContentType != "image/jpeg" && Product.PosterImageFile.ContentType != "image/png")
            {
                throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("PosterImageFile", "Content type must be png or jpeg!");
            }

            if (Product.PosterImageFile.Length > 2097152)
            {
                throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("PosterImageFile", "Size must be lower than 2mb!");
            }
            Models.ProductImage posterImage = new Models.ProductImage()
            {
                Product = Product,
                ImageUrl = Product.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/Products"),
                IsPoster = true
            };
            await _context.ProductImages.AddAsync(posterImage);

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
                Product = Product,
                ImageUrl = Product.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/Products"),
                IsPoster = false
            };
            await _context.ProductImages.AddAsync(hoverImage);


            if (Product.ImageFiles is not null)
            {
                foreach (var imageFile in Product.ImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("ImageFiles", "Content type must be png or jpeg!");
                    }

                    if (imageFile.Length > 2097152)
                    {
                        throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("ImageFiles", "Size must be lower than 2mb!");
                    }
                    ProductImage productImage = new ProductImage()
                    {
                        Product = Product,
                        ImageUrl = imageFile.SaveFile(_env.WebRootPath, "uploads/Products"),
                        IsPoster = null
                    };
                    await _context.ProductImages.AddAsync(productImage);
                }
            }

            await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();
        }

        public  async Task DeleteAsync(int id)
        {
            var data = await _context.Products.FindAsync(id);
            if (data is null) throw new CustomExceptions.CategoryExceptions.CategoryNotFoundException("product not found!");

            _context.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Products.AsQueryable();

            query = _getIncludes(query, includes);

            return expression is not null
                        ? await query.Where(expression).OrderByDescending(x => x.Id).ToListAsync()
                        : await query.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var query = _context.Products.Include(x=>x.ProductImages).Where(x=>x.Id==id).AsQueryable();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetSingleAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Products.AsQueryable();

            query = _getIncludes(query, includes);

            return expression is not null
                        ? await query.Where(expression).FirstOrDefaultAsync()
                        : await query.FirstOrDefaultAsync();
        }

        public Task SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Product> _getIncludes(IQueryable<Product> query, params string[] includes)
        {
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
