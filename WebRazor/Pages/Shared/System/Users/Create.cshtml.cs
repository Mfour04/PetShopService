using System.Security.Policy;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using WebRazor.Models.Services;

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

        [BindProperty]
        public User User { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || User == null)
            {
                return Page();
            }

            // Kiểm tra nếu email đã tồn tại
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == User.Email);
            if (existingUser != null)
            {
                ErrorMessage = "Email đã tồn tại. Vui lòng nhập một email khác.";
                return Page();
            }

            // Thiết lập RoleId và tạo token xác thực
            User.RoleId = "user";
            User.EmailVerificationToken = Guid.NewGuid().ToString();
            User.IsEmailVerified = false;

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            // Kiểm tra lại token sau khi lưu
            var savedUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == User.Email);
            if (savedUser == null || string.IsNullOrEmpty(savedUser.EmailVerificationToken))
            {
                ErrorMessage = "Có lỗi xảy ra khi lưu người dùng hoặc token xác thực.";
                return Page();
            }

            // Tạo liên kết xác thực
            var verificationLink = Url.Page("/Shared/System/Users/VerifyEmail", null, new { token = User.EmailVerificationToken }, Request.Scheme);

            // Gửi email xác thực
            await _emailService.SendVerificationEmail(User.Email, verificationLink);

            Console.WriteLine($"Verification link: {verificationLink}");

            return RedirectToPage("/Shared/System/Users/AfterRegister");
        }

    }
}