using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetShopLibrary.Models;
using PetShopLibrary.Service;
using System.Security.Claims;

namespace WebRazor.Pages
{
    public class ViewBookingModel : PageModel
    {
        private readonly ServiceScheduleService _serviceScheduleService;
        private readonly ShopServiceService _shopServiceService;

        public ViewBookingModel(ServiceScheduleService serviceScheduleService, ShopServiceService shopServiceService)
        {
            _serviceScheduleService = serviceScheduleService;
            _shopServiceService = shopServiceService;
        }

        [BindProperty]
        public long UserId { get; set; }

        [BindProperty]
        public DateTime Date { get; set; } = DateTime.Today;

        public List<ServiceSchedule> ServiceSchedules { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy thông tin người dùng từ claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                UserId = long.Parse(userIdClaim.Value);
            }
            // Lấy lịch trình dịch vụ của người dùng
            ServiceSchedules = new List<ServiceSchedule>(await _serviceScheduleService.GetSchedulesByUserIdAsync(UserId));


            return Page();
        }
    }
}
