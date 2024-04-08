using AllupP238.Models;
using System.Linq.Expressions;

namespace AllupP238.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null);
        Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes); 
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);   
    }
}
