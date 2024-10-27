using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt; // Cần thêm namespace này để làm việc với JWT
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WebRazor.Pages.Shared.Customer
{
    [Authorize]
    public class CustomerHomeModel : PageModel
	{
		private readonly ILogger<CustomerHomeModel> _logger; // Khai báo biến logger

		public CustomerHomeModel(ILogger<CustomerHomeModel> logger)
		{
			_logger = logger; // Gán logger
		}

		public string Email { get; set; }

		public string UserId { get; set; }

		public void OnGet()
		{
            // Lấy thông tin từ các claim đã được lưu trữ trong Cookie Authentication
            var emailClaim = User.FindFirst(ClaimTypes.Name);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (emailClaim != null)
            {
                Email = emailClaim.Value;
                _logger.LogInformation($"Email: {Email}");
            }
            else
            {
                _logger.LogWarning("Email claim is not available.");
            }

            if (userIdClaim != null)
            {
                UserId = userIdClaim.Value;
                _logger.LogInformation($"UserId: {UserId}");
            }
            else
            {
                _logger.LogWarning("UserId claim is not available.");
            }
        }
	}
}