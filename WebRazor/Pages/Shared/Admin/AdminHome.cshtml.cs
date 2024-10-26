using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Shared.Admin
{
    [Authorize(Roles = "admin")]  // Chỉ cho phép admin truy cập
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
            // Ghi log các claim của người dùng
            var userClaims = User.Claims.ToList();

            // Ghi log khi người dùng truy cập AdminHome
            _logger.LogInformation("User attempting to access AdminHome");
            _logger.LogInformation("Is User Authenticated: " + User.Identity.IsAuthenticated);
            _logger.LogInformation("Claims Count: " + User.Claims.Count());

            foreach (var claim in userClaims)
            {
                _logger.LogInformation($"Claim type: {claim.Type}, value: {claim.Value}");
            }


            return Page();
		}
    }
}
