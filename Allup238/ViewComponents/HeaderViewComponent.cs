using AllupP238.Business.Interfaces;
using AllupWebApplication.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllUpMVC.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICategoryService _CategoryService;
        public HeaderViewComponent(ICategoryService categoryService)
        {
            _CategoryService = categoryService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categorys = await _CategoryService.GetAllAsync(null);
            return View(categorys);
        }
    }
}
