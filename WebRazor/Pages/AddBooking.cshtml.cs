using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PetShopLibrary.Models;
using PetShopLibrary.Service;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace WebRazor.Pages
{
    public class AddBookingModel : PageModel
    {
        private readonly ServiceScheduleService _serviceScheduleService;
        private readonly ShopServiceService _shopServiceService;
        private readonly UserService _userService;

        public AddBookingModel(ServiceScheduleService serviceScheduleService, ShopServiceService shopServiceService, UserService userService)
        {
            _serviceScheduleService = serviceScheduleService;
            _shopServiceService = shopServiceService;
            _userService = userService;
        }


        [BindProperty]
        public ServiceSchedule NewSchedule { get; set; }

        public IEnumerable<ShopService> Services { get; set; }

        public User Users { get; set; }

        public async Task OnGetAsync()
        {
            Services = await _shopServiceService.GetAllServicesAsync();
        }

        public async Task<IActionResult> OnPostPreviewAsync()
        {
            Services = await _shopServiceService.GetAllServicesAsync();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return new JsonResult(new { success = false, message = "User ID not found. Please log in." });
            }

            NewSchedule.UserId = long.Parse(userIdClaim.Value);
            NewSchedule.Status = 1;

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data." });
            }

            var selectedService = await _shopServiceService.GetServiceByIdAsync(NewSchedule.ServiceId ?? 0);
            if (selectedService == null)
            {
                return new JsonResult(new { success = false, message = "Service not found." });
            }


            // Trả về dữ liệu dịch vụ và ngày đã chọn
            return new JsonResult(new
            {
                success = true,
                selectedServiceName = selectedService.ServiceName,
                selectedServicePrice = selectedService.Price,
                selectedDate = NewSchedule.Date?.ToString("yyyy-MM-dd"),
                selectedStatus = NewSchedule.Status,
                selectedUserId = NewSchedule.UserId
            });
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            if (NewSchedule != null)
            {
                // Lưu lịch hẹn vào database
                await _serviceScheduleService.AddScheduleAsync(NewSchedule);

                // Gửi email xác nhận
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return new JsonResult(new { success = false, message = "User ID not found. Please log in." });
                }

                Users = _userService.GetUserById(long.Parse(userIdClaim));

                if (!string.IsNullOrEmpty(Users.Email))
                {
                    var selectedService = await _shopServiceService.GetServiceByIdAsync(NewSchedule.ServiceId ?? 0);
                    if (selectedService != null)
                    {
                        await SendEmailAsync(
                            Users.Email,
                            selectedService.ServiceName,
                            selectedService.Price,
                            NewSchedule.Date?.ToString("yyyy-MM-dd")
                        );
                    }
                }

                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false });
        }

        private async Task SendEmailAsync(string toEmail, string serviceName, long? price, string date)
        {
            try
            {
                // Sử dụng 'using' để đảm bảo tài nguyên được giải phóng đúng cách
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("shoppetpath@gmail.com", "dqoympvraxhgjmkr");
                    smtpClient.EnableSsl = true;

                    // Tạo nội dung email
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("shoppetpath@gmail.com", "PetPath Team"),
                        Subject = "Your Pet Grooming Schedule Confirmation",
                        Body = $@"
                    <h2>Pet Grooming Schedule Confirmation</h2>
                    <p>Dear customer,</p>
                    <p>Thank you for scheduling a service with us!</p>
                    <p><strong>Service:</strong> {serviceName}</p>
                    <p><strong>Price:</strong> ${(price ?? 0):F2}</p>
                    <p><strong>Date:</strong> {date}</p>
                    <p>We look forward to seeing you soon!</p>
                    <p>Best regards,<br>PetPath Team</p>",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    // Gửi email
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để bạn có thể theo dõi và sửa chữa
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

    }
}
