using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly ILogger<LogoutModel> _logger;

		public LogoutModel(ILogger<LogoutModel> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> OnGet() // should convert to OnPost
		{
			_logger.LogInformation("User logged out.");

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			// Chuyển hướng về trang đăng nhập sau khi logout
			return RedirectToPage("/Login");
		}
	}
}
