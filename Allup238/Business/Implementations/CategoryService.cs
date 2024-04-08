using Microsoft.EntityFrameworkCore;
using AllupP238.Business.Interfaces;
using AllupP238.CustomExceptions.Common;
using AllupP238.CustomExceptions.GenreExceptions;
using AllupP238.Data;
using AllupP238.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AllupDbContext _context;

        public CategoryService(AllupDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category category)
        {
            if (_context.Categories.Any(x => x.Name.ToLower() == category.Name.ToLower()))
                throw new NameAlreadyExistException("Name","Genre name is already exist!");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public Task CreateAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data is null) throw new GenreNotFoundException("Genre not found!");

            _context.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes) // isdeleted = false
        {
            var query = _context.Categories.AsQueryable(); // Select * from Genres

            query = _getIncludes(query, includes);

            return expression is not null 
                    ? await query.Where(expression).ToListAsync()  // Select * From Genres Where EXPRESSION
                    : await query.ToListAsync(); // SELECT * FROM Genres
        }

        public Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data is null) throw new CategoryNotFoundException();

            return data;
        }

        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
        {
            var query = _context.Categories.AsQueryable();

            return expression is not null
                    ? await query.Where(expression).FirstOrDefaultAsync()
                    : await query.FirstOrDefaultAsync();
        }

        public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null)
        {
            throw new NotImplementedException();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data is null) throw new CategoryNotFoundException();
            data.IsDeleted = !data.IsDeleted;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            var existData = await _context.Categories.FindAsync(category.Id);
            if (existData is null) throw new CategoryNotFoundException("Category not found!");
            if (_context.Categories.Any(x => x.Name.ToLower() == category.Name.ToLower()) 
                && existData.Name != category.Name)
                throw new NameAlreadyExistException("Name", "Category name is already exist!");

            existData.Name = category.Name;
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Category category)
        {
            throw new NotImplementedException();
        }

        Task<Category> ICategoryService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        //private IQueryable<Genre> _getQuery(IQueryable<Genre> query, Expression<Func<Genre, bool>>? expression = null)
        //{
        //    if (expression is not null)
        //        query = query.Where(expression);

        //    return query;
        //}
    }
}
