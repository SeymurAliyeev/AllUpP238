using AllupP238.Business.Implementations;
using AllupP238.Business.Interfaces;

namespace AllupMVC;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISliderServices, SliderService>();
    }
}
