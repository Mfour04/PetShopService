using System.Security.Policy;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;

namespace WebRazor.Pages.Shared.System.Users
{
    public class CreateModel : PageModel
    {
        private readonly PetShopContext _context;
        private readonly EmailService _emailService;

        public CreateModel(PetShopContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || User == null)
            {
                return Page();
            }

            // Thiết lập RoleId và tạo token xác thực
            User.RoleId = "user";
            User.EmailVerificationToken = Guid.NewGuid().ToString();
            User.IsEmailVerified = false;

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            // Tạo liên kết xác thực
            var verificationLink = Url.Page("/Shared/System/VerifyEmail", null, new { token = User.EmailVerificationToken }, Request.Scheme);

            // Gửi email xác thực
            await _emailService.SendVerificationEmail(User.Email, verificationLink);

            return RedirectToPage("./Index");
        }


        private async Task SendVerificationEmail(string email, string token)
        {
            var verificationLink = Url.Page("/Account/VerifyEmail", null, new { token = token }, Request.Scheme);
            var subject = "Xác thực tài khoản của bạn";
            var message = $"Vui lòng xác thực email của bạn bằng cách nhấp vào liên kết sau: {verificationLink}";

            // Gửi email sử dụng SMTP hoặc dịch vụ email khác
        }
    }
}