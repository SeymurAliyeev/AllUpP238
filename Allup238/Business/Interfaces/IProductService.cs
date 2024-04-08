using AllupP238.Models;
using System.Linq.Expressions;

namespace AllupP238.Business.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetSingleAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes);
        Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
