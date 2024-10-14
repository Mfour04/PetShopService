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

		public IActionResult OnGet()
		{
			// Xóa cookie chứa token
			HttpContext.Response.Cookies.Delete("jwtToken");

			_logger.LogInformation("User logged out.");

			// Chuyển hướng về trang đăng nhập sau khi logout
			return RedirectToPage("/Login");
		}
	}
}
