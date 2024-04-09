using Microsoft.EntityFrameworkCore;
using AllupP238.Business.Interfaces;
using AllupP238.CustomExceptions.Common;
using AllupP238.CustomExceptions.CategoryExceptions;
using AllupP238.Data;
using AllupP238.Models;
using System.Linq.Expressions;
using AllupP238.CustomExceptions.ProductExceptions;
using AllupP238.Extensions;
using System.ComponentModel;

namespace AllUpMVC.Business.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;


        public CategoryService(AllupDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(Category Category)
        {
            if (_context.Categories.Any(x => x.Name.ToLower() == Category.Name.ToLower()))
                throw new NameAlreadyExistException("Name","Category name is already exist!");

            if (Category.CategoryImageFile.ContentType != "image/jpeg" && Category.CategoryImageFile.ContentType != "image/png")
            {
                throw new CategoryInvalidCredentialException("CategoryImageFile", "Content type must be png or jpeg!");
            }

            if (Category.CategoryImageFile.Length > 2097152)
            {
                throw new CategoryInvalidCredentialException("CategoryImageFile", "Size must be lower than 2mb!");
            }

            Category.CategoryImage = Category.CategoryImageFile.SaveFile(_env.WebRootPath, "uploads/categorys");
            


            await _context.Categorys.AddAsync(Category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Categorys.FindAsync(id);
            if (data is null) throw new CategoryNotFoundException("Category not found!");

            _context.Categorys.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckChildAsync(int CategoryId)
        {
            var data=await _context.Products.Where(x=>x.CategoryId == CategoryId).ToListAsync();
            if (data.Count()!=0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }

        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes) 
        {
            var query = _context.Categorys.AsQueryable();

            query = _getIncludes(query, includes);

            return expression is not null 
                    ? await query.Where(expression).ToListAsync()  
                    : await query.ToListAsync(); 
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var data = await _context.Categorys.FindAsync(id);
            if (data is null) throw new CategoryNotFoundException();

            return data;
        }

        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
        {
            var query = _context.Categorys.AsQueryable();

            return expression is not null
                    ? await query.Where(expression).FirstOrDefaultAsync()
                    : await query.FirstOrDefaultAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var data = await _context.Categorys.FindAsync(id);
            if (data is null) throw new CategoryNotFoundException();
            data.IsDeleted = !data.IsDeleted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category Category)
        {
            var existData = await _context.Categorys.FindAsync(Category.Id);
            if (existData is null) throw new CategoryNotFoundException("Category not found!");
            if (_context.Categorys.Any(x => x.Name.ToLower() == Category.Name.ToLower()) 
                && existData.Name != Category.Name)
                throw new NameAlreadyExistException("Name", "Category name is already exist!");

            existData.Name = Category.Name;
            if (Category.CategoryImageFile is not null)
            {
                if (Category.CategoryImageFile.ContentType != "image/jpeg" && Category.CategoryImageFile.ContentType != "image/png")
                {
                    throw new ProductInvalidCredentialException("CategoryImageFile", "Content type must be png or jpeg!");
                }

                if (Category.CategoryImageFile.Length > 2097152)
                {
                    throw new ProductInvalidCredentialException("CategoryImageFile", "Size must be lower than 2mb!");
                }
                existData.CategoryImage = Category.CategoryImageFile.SaveFile(_env.WebRootPath, "uploads/categorys");

            }

            await _context.SaveChangesAsync();
        }


        private IQueryable<Category> _getIncludes(IQueryable<Category> query, params string[] includes)
        {
            if(includes is not null)
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
