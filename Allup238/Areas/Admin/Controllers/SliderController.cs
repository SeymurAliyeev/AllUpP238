using AllupP238.Business.Interfaces;
using AllupP238.Models;
using Microsoft.AspNetCore.Mvc;

namespace AllupP238.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderServices _sliderService;

        public SliderController(ISliderServices sliderService)
        {
            _sliderService = sliderService;
        }

        // Display the list of sliders
        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllSlidersAsync();
            return View(sliders);
        }

        // Show the form to create a new slider
        public IActionResult Create()
        {
            return View();
        }

        // Process the creation of a new slider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _sliderService.CreateSliderAsync(slider, imageFile);
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // Show the form to edit an existing slider
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _sliderService.GetSliderByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // Process the update of an existing slider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider, IFormFile? imageFile = null)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _sliderService.UpdateSliderAsync(slider, imageFile);
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // Soft delete a slider (mark as inactive)
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _sliderService.SoftDeleteSliderAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Confirm deletion page for hard delete
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var slider = await _sliderService.GetSliderByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // Hard delete a slider (remove from database)
        [HttpGet, ActionName("HardDeleteConfirmed")]

        public async Task<IActionResult> HardDeleteConfirmed(int id)
        {
            await _sliderService.HardDeleteSliderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
