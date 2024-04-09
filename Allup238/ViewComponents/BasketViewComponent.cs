using AllUpMVC.Business.Interfaces;
using AllUpMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AllUpMVC.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemsStr is not null)
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr, settings);
            }
            return View(basketItems);
        }
    }
}