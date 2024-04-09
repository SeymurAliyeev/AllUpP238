using AllUpMVC.Business.Implementations;
using AllupP238.Business.Interfaces;
using AllupP238.Business.Implementations;
using AllupP238.Business.Interfaces;
using AllupWebApplication.Business.Interfaces;

namespace AllUpMVC
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISliderServices, SliderService>();
            services.AddHttpContextAccessor();

        }
    }
}
