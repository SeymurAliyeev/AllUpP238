using AllUpMVC.Models;

namespace AllUpMVC.ViewModels
{
    public class BasketItemViewModel
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }    
        public int Count { get; set; }
    }
}