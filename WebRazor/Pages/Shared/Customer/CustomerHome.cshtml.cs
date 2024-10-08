using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt; // Cần thêm namespace này để làm việc với JWT
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Shared.Customer
{
	public class CustomerHomeModel : PageModel
	{
		private readonly ILogger<CustomerHomeModel> _logger; // Khai báo biến logger

		public CustomerHomeModel(ILogger<CustomerHomeModel> logger)
		{
			_logger = logger; // Gán logger
		}

		public string Email { get; set; }

		public void OnGet()
		{
			// Lấy JWT token từ Cookie
			var jwtToken = Request.Cookies["jwtToken"];

			if (!string.IsNullOrEmpty(jwtToken))
			{
				// Log JWT token
				_logger.LogInformation($"JWT Token: {jwtToken}");

				// Giải mã token để lấy email
				var handler = new JwtSecurityTokenHandler();
				var token = handler.ReadJwtToken(jwtToken);

				_logger.LogInformation($"Token: {token}");

				// Lấy email từ token
				var emailClaim = token.Claims.FirstOrDefault(c => c.Type == "unique_name");
				if (emailClaim != null)
				{
					Email = emailClaim.Value;
				}
			}
			else
			{
				_logger.LogWarning("JWT Token is not available in the cookies.");
			}
		}
	}
}