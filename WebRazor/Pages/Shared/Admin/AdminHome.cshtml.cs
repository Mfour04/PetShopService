using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Shared.Admin
{
	/*[Authorize(Roles = "admin")]*/  // Chỉ cho phép admin truy cập
	public class AdminHomeModel : PageModel
    {
		private readonly ILogger<AdminHomeModel> _logger; // Tạo biến logger

		// Inject ILogger vào thông qua constructor
		public AdminHomeModel(ILogger<AdminHomeModel> logger)
		{
			_logger = logger; // Gán logger
		}

		public async Task<IActionResult> OnGet()
        {
			// Ghi log khi người dùng truy cập AdminHome
			_logger.LogInformation("User attempting to access AdminHome");

			// Ghi log các claim của người dùng
			var userClaims = User.Claims.ToList();

			_logger.LogInformation("userClaims: " + userClaims); // value = System.Collections.Generic.List`1[System.Security.Claims.Claim]

			_logger.LogInformation("userClaimsLength: " + userClaims.Count); // value = 0

			return Page();
		}
    }
}
