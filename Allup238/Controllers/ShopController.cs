using AllUpMVC.Business.Implementations;
using AllUpMVC.Business.Interfaces;
using AllUpMVC.Data;
using AllUpMVC.Models;
using AllUpMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AllUpMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly AllUpDbContext _context;
        private List<Basketitem> _basket = new List<Basketitem>();
        private const string BasketCookieName = "Basket";

        public ShopController(
                IProductService ProductService,
                AllUpDbContext context)
        {
            _ProductService = ProductService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> RemoveFromBasket(int productId)
        {
            var product = await _ProductService.GetByIdAsync(productId);

            if (product is null) return NotFound();

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            BasketItemViewModel basketItem = null;

            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
                basketItem = basketItems.FirstOrDefault(x => x.ProductId == productId);
                if (basketItemsStr is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
                    basketItem = basketItems.FirstOrDefault(x => x.ProductId == productId);
                    if (basketItem is not null)
                    {
                        basketItems.Remove(basketItem);
                    }
                }
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            basketItemsStr = JsonConvert.SerializeObject(basketItems, settings);
            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);
            // Redirect to action to display the basket
            return Redirect("/Home/Index");
        }
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var product = await _ProductService.GetByIdAsync(productId);

            if (product is null) return NotFound();
            Product existProduct = await _ProductService.GetSingleAsync(x => x.Id == productId, "ProductImages");

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            BasketItemViewModel basketItem = null;

            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
                basketItem = basketItems.FirstOrDefault(x => x.ProductId == productId);
                if (basketItemsStr is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
                    basketItem=basketItems.FirstOrDefault(x => x.ProductId == productId);
                    if (basketItem is not null)
                    {
                        basketItem.Count++;
                    }
                    else
                    {
                         basketItem = new BasketItemViewModel()
                        {
                             Product =existProduct,
                            ProductId = productId,
                            Count = 1
                        };
                        basketItems.Add(basketItem);
                    }
                }
            }
            else
            {
                basketItem = new BasketItemViewModel()
                {
                    Product = existProduct,
                    ProductId = productId,
                    Count = 1
                };
                basketItems.Add(basketItem);
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            basketItemsStr = JsonConvert.SerializeObject(basketItems,settings);
            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);
            return Ok();
        }
        public IActionResult GetBasketItems()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
            return Ok(basketItems);
        }

    }
}
