using AllUpMVC.Business.Interfaces;
using AllUpMVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace AllUpMVC.Controllers;

public class PartialController : Controller
{
    private readonly AllUpDbContext _context;
    private readonly ICategoryService _CategoryService;
    private readonly IProductService _ProductService;
    private readonly ISliderService _SliderService;

    public PartialController(
            AllUpDbContext context,
            ICategoryService CategoryService,
            IProductService ProductService, ISliderService SliderService)
    {
        _context = context;
        _CategoryService = CategoryService;
        _ProductService = ProductService;
        _SliderService = SliderService;
    }

    public async Task<IActionResult> MyPartialView()
    {
        var categorys = _context.Categorys.ToList();
        return PartialView("_CategoryPartial", categorys);

    }
}
