using AllupP238.Business.Interfaces;
using AllupP238.Data;
using AllupP238.Models;
using AllupWebApplication.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AllupP238.Business.Implementations
{
    public class SliderService: ISliderServices
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderService(AllupDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Slider>> GetAllSlidersAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<IEnumerable<Slider>> GetActiveSlidersAsync()
        {
            return await _context.Sliders
                                 .Where(s => s.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<Slider> GetSliderByIdAsync(int id)
        {
            return await _context.Sliders
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateSliderAsync(Slider slider, IFormFile imageFile)
        {
            // Handling file upload
            slider.ImageUrl = await FileManager.SaveFileAsync(imageFile, _webHostEnvironment.WebRootPath, "uploads/sliders");

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSliderAsync(Slider slider, IFormFile? imageFile = null)
        {
            var existingSlider = await _context.Sliders.FindAsync(slider.Id);
            if (existingSlider != null)
            {
                existingSlider.Title = slider.Title;
                existingSlider.Description = slider.Description;
                existingSlider.ButtonText = slider.ButtonText;
                existingSlider.ButtonUrl = slider.ButtonUrl;

                // Handle image update
                if (imageFile != null)
                {
                    FileManager.DeleteFile(_webHostEnvironment.WebRootPath, "uploads/sliders", existingSlider.ImageUrl);
                    existingSlider.ImageUrl = await FileManager.SaveFileAsync(imageFile, _webHostEnvironment.WebRootPath, "uploads/sliders");
                }

                _context.Sliders.Update(existingSlider);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteSliderAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider != null)
            {
                slider.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task HardDeleteSliderAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider != null)
            {
                _context.Sliders.Remove(slider);
                await _context.SaveChangesAsync();
            }
        }
    }
}
