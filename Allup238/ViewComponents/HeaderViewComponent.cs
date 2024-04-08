using Microsoft.AspNetCore.Mvc;
using AllupP238.Business.Interfaces;

namespace PustokMVC.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICategoryService _CategoryService;

        public HeaderViewComponent(ICategoryService CategoryService)
        {
            _CategoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllAsync(null);

            return View(categories);
        }
    }
}
