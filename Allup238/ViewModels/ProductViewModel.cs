using AllUpMVC.Models;
using AllupWebApplication.Models;

namespace AllUpMVC.ViewModels
{
    public class ProductViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
