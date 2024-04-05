using AllupP238.Models;

namespace AllupP238.Business.Interfaces
{
    public interface ISliderServices
    {
        Task<IEnumerable<Slider>> GetAllSlidersAsync();
        Task<IEnumerable<Slider>> GetActiveSlidersAsync();
        Task<Slider> GetSliderByIdAsync(int id);


        Task CreateSliderAsync(Slider slider, IFormFile imageFile);
        Task UpdateSliderAsync(Slider slider, IFormFile? imageFile = null);

        Task SoftDeleteSliderAsync(int id);
        Task HardDeleteSliderAsync(int id);
    }
}
