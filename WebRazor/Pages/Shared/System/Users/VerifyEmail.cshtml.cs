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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
            if (user == null || user.IsEmailVerified)
            {
                return NotFound("Liên kết xác thực không hợp lệ hoặc đã được sử dụng.");
            }

            // Xác thực email
            user.IsEmailVerified = true;
            user.EmailVerificationToken = null; // Xóa token sau khi xác thực
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login");
        }
    }
}
