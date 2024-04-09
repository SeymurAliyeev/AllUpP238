using AllUpMVC.Models;
using AllupP238.Business.Interfaces;
using AllupP238.Data;
using AllupP238.Helpers.Extensions;
using AllupWebApplication.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUpMVC.Controllers;

public class ProductController : Controller
{
    private readonly AllupDbContext _context;
    private readonly ICategoryService _CategoryService;
    private readonly IProductService _ProductService;
    private readonly ISliderServices _SliderService;

    public ProductController(
            AllupDbContext context, 
            ICategoryService CategoryService,
            IProductService ProductService,ISliderServices SliderService)
    {
        _context = context;
        _CategoryService = CategoryService;
        _ProductService = ProductService;
        _SliderService= SliderService;  
    }
    public async Task<IActionResult> Detail(int productId)
    {
        Product product = await _ProductService.GetSingleAsync(x => x.Id == productId, "ProductImages");

        return View(product);
    }

    public async Task<IActionResult> Index(int page,int ? CategoryId)
    {
        ViewBag.Categories = await _CategoryService.GetAllAsync(x => x.IsDeleted == false);

        var datas = _context.Products.AsQueryable();
        if (CategoryId != null)
        {
            datas = datas.Include(x => x.Category).Include(x => x.ProductImages).Where(x => x.CategoryId == CategoryId).OrderByDescending(x => x.Id);
        }
        else
        {
            datas = datas.Include(x => x.Category).Include(x => x.ProductImages).OrderByDescending(x => x.Id);
        }
        var paginatedDatas = PaginatedList<Product>.Create(datas, 2, page);
         return View(paginatedDatas);
     
    }
}
