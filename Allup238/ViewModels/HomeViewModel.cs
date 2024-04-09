using AllUpMVC.Models;
using Humanizer.Localisation;

namespace AllUpMVC.ViewModels;

public class HomeViewModel
{
    public List<Slider> Sliders { get; set; }
    public List<Category> Categories { get; set; }
    public List<Product> FeaturedProducts { get; set; }
    public List<Product> NewProducts { get; set; }
    public List<Product> BestSellerProducts { get; set; }
}
