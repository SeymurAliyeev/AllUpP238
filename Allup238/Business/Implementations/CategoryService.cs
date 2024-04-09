//using AllupP238.Models;
//using AllupP238.Business.Interfaces;
//using AllupP238.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;
//using AllupP238.Models;

//namespace AllupP238.Business.Implementations
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly AllupDbContext _context;
//        private readonly IWebHostEnvironment _env;


//        public CategoryService(AllupDbContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }

//        public async Task CreateAsync(Category Category)
//        {
//            if (_context.Categories.Any(x => x.Name.ToLower() == Category.Name.ToLower()))
//                throw new CustomExceptions.Common.NameAlreadyExistException("Name", "Category name is already exist!");

//            if (Category.CategoryImageFile.ContentType != "image/jpeg" && Category.CategoryImageFile.ContentType != "image/png")
//            {
//                throw new CustomExceptions.CategoryExceptions.CategoryInvalidCredentialException("CategoryImageFile", "Content type must be png or jpeg!");
//            }

//            if (Category.CategoryImageFile.Length > 2097152)
//            {
//                throw new CustomExceptions.CategoryExceptions.CategoryInvalidCredentialException("CategoryImageFile", "Size must be lower than 2mb!");
//            }

//            Category.CategoryImage = Category.CategoryImageFile.SaveFile(_env.WebRootPath, "uploads/categories");



//            await _context.Categories.AddAsync(Category);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var data = await _context.Categories.FindAsync(id);
//            if (data is null) throw new CustomExceptions.CategoryExceptions.CategoryNotFoundException("Category not found!");

//            _context.Categories.Remove(data);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<bool> CheckChildAsync(int CategoryId)
//        {
//            var data=await _context.Products.Where(x=>x.CategoryId == CategoryId).ToListAsync();
//            if (data.Count()!=0)
//            {
//                return false;

//            }
//            else
//            {
//                return true;
//            }
//        }

//        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes) 
//        {
//            var query = _context.Categories.AsQueryable();

//            query = _getIncludes(query, includes);

//            return expression is not null 
//                    ? await query.Where(expression).ToListAsync()  
//                    : await query.ToListAsync(); 
//        }

//        public async Task<Category> GetByIdAsync(int id)
//        {
//            var data = await _context.Categories.FindAsync(id);
//            if (data is null) throw new CustomExceptions.CategoryExceptions.CategoryNotFoundException();

//            return data;
//        }

//        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
//        {
//            var query = _context.Categories.AsQueryable();

//            return expression is not null
//                    ? await query.Where(expression).FirstOrDefaultAsync()
//                    : await query.FirstOrDefaultAsync();
//        }

//        public async Task SoftDeleteAsync(int id)
//        {
//            var data = await _context.Categories.FindAsync(id);
//            if (data is null) throw new CustomExceptions.CategoryExceptions.CategoryNotFoundException();
//            data.IsDeleted = !data.IsDeleted;

//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(Category Category)
//        {
//            var existData = await _context.Categories.FindAsync(Category.Id);
//            if (existData is null) throw new CustomExceptions.CategoryExceptions.CategoryNotFoundException("Category not found!");
//            if (_context.Categories.Any(x => x.Name.ToLower() == Category.Name.ToLower()) 
//                && existData.Name != Category.Name)
//                throw new CustomExceptions.Common.NameAlreadyExistException("Name", "Category name is already exist!");

//            existData.Name = Category.Name;
//            if (Category.CategoryImageFile is not null)
//            {
//                if (Category.CategoryImageFile.ContentType != "image/jpeg" && Category.CategoryImageFile.ContentType != "image/png")
//                {
//                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("CategoryImageFile", "Content type must be png or jpeg!");
//                }

//                if (Category.CategoryImageFile.Length > 2097152)
//                {
//                    throw new CustomExceptions.ProductExceptions.ProductInvalidCredentialException("CategoryImageFile", "Size must be lower than 2mb!");
//                }
//                existData.CategoryImage = Category.CategoryImageFile.SaveFile(_env.WebRootPath, "uploads/categorys");

//            }

//            await _context.SaveChangesAsync();
//        }


//        private IQueryable<Category> _getIncludes(IQueryable<Category> query, params string[] includes)
//        {
//            if(includes is not null)
//            {
//                foreach (var include in includes)
//                {
//                    query = query.Include(include);
//                }
//            }
//            return query;
//        }

//        Task<Category> ICategoryService.GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        Task<Category> ICategoryService.GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        Task<Category> ICategoryService.GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        Task<Category> ICategoryService.GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(Category category)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}


using AllUpMVC.Extensions;
using AllupP238.Data;
using AllupWebApplication.Business.Interfaces;
using AllupWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class CategoryService : ICategoryService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CategoryService(AllupDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    private IQueryable<Category> _getIncludes(IQueryable<Category> query, params string[] includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? filter = null, params string[] includes)
    {
        IQueryable<Category> query = _context.Categories;
        query = _getIncludes(query, includes);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _context.Categories
                             .Where(c => c.IsDeleted)
                             .ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task CreateCategoryAsync(Category category, IFormFile imageFile)
    {
        string folderPath = "uploads/categories";
        category.ImageUrl = await FileManager.SaveFileAsync(imageFile, _webHostEnvironment.WebRootPath, folderPath);


        object value = await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category, IFormFile? imageFile = null)
    {
        var existingCategory = await _context.Categories.FindAsync(category.Id);
        if (existingCategory != null)
        {
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.IsDeleted = category.IsDeleted;

            if (imageFile != null)
            {
                FileManager.DeleteFile(_webHostEnvironment.WebRootPath, "uploads/categories", existingCategory.ImageUrl);
                existingCategory.ImageUrl = await FileManager.SaveFileAsync(imageFile, _webHostEnvironment.WebRootPath, "uploads/categories");
            }

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SoftDeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null && category.IsDeleted == false)
        {
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task HardDeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }
        if (category.IsDeleted)
        {
            throw new InvalidOperationException("Cannot hard delete an active category. Please deactivate the category first.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public Task<Category> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task SoftDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<dynamic> GetAllAsync(Func<object, bool> value)
    {
        throw new NotImplementedException();
    }
}

