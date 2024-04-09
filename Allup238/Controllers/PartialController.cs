using AllupP238.Business.Interfaces;
using AllupP238.Data;
using AllupWebApplication.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllUpMVC.Controllers;

public class PartialController : Controller
{
    private readonly AllupDbContext _context;
    private readonly ICategoryService _CategoryService;
    private readonly IProductService _ProductService;
    private readonly ISliderServices _SliderService;

    public PartialController(
            AllupDbContext context,
            ICategoryService CategoryService,
            IProductService ProductService, ISliderServices SliderService)
    {
        _context = context;
        _CategoryService = CategoryService;
        _ProductService = ProductService;
        _SliderService = SliderService;
    }

    public async Task<IActionResult> MyPartialView()
    {
        var categorys = _context.Categories.ToList();
        return PartialView("_CategoryPartial", categorys);

    }
}
