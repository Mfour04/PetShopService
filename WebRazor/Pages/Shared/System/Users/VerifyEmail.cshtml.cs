using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;

namespace WebRazor.Pages.Shared.System.Users
{
    public class VerifyEmailModel : PageModel
    {
        private readonly PetShopContext _context;

        public VerifyEmailModel(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound("Liên kết xác thực không hợp lệ.");
            }

            Console.WriteLine($"Token received for verification: {token}");

            try
            {
                // Tìm người dùng với token đã cho
                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
                if (user == null)
                {
                    return NotFound("Liên kết xác thực không hợp lệ hoặc đã được sử dụng.");
                }

                if (user.IsEmailVerified)
                {
                    return NotFound("Email đã được xác thực trước đó.");
                }

                // Xác thực email
                user.IsEmailVerified = true;
                user.EmailVerificationToken = null; // Xóa token sau khi xác thực

                _context.Users.Attach(user); // Đảm bảo rằng người dùng được theo dõi bởi context
                _context.Entry(user).State = EntityState.Modified; // Đánh dấu người dùng là đã sửa đổi

                await _context.SaveChangesAsync();

                Console.WriteLine("Email đã được xác thực thành công.");

                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while verifying email: {ex.Message}");
                return StatusCode(500, "Đã có lỗi xảy ra trong quá trình xác thực email.");
            }
        }

    }
}
