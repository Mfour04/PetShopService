using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.Dynamic;

namespace WebRazor.Pages
{
    public class BookingServiceModel : PageModel
    {
        private readonly ServiceScheduleService _serviceScheduleService;

        public List<ShopService> ShopServices { get; set; }
        public List<ServiceSchedule> SchedulesForSelectedDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public BookingServiceModel(ServiceScheduleService serviceScheduleService)
        {
            _serviceScheduleService = serviceScheduleService;
        }

        public void OnGet() {
            ShopServices = _serviceScheduleService.GetAllServices();

            SchedulesForSelectedDate = _serviceScheduleService.GetSchedulesByDate(SelectedDate);
        }

        public IActionResult OnPostAddSchedule(long serviceId, DateTime date)
        {

            // Kiểm tra tính hợp lệ của serviceId và date trước khi thêm lịch trình
            if (serviceId <= 0 || date == default)
            {
                ModelState.AddModelError(string.Empty, "Invalid service or date.");
                return Page();
            }


            _serviceScheduleService.AddServiceSchedule(new ServiceSchedule
            {
                ServiceId = serviceId,
                Date = date,
                Status = 1
            });
            return RedirectToPage("BookingService");
        }
    }
}
