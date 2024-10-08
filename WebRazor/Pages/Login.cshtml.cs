using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebRazor.Models.AuthenticationModel;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public class LoginModel : PageModel
{
	private readonly PetShopContext _context;

	private readonly ILogger<LoginModel> _logger; // Thêm biến logger

	private readonly IConfiguration _configuration;

	public LoginModel(PetShopContext context, ILogger<LoginModel> logger, IConfiguration configuration)
	{
		_context = context;
		_logger = logger; // Gán logger
		_configuration = configuration;
	}

	[BindProperty]
	public LoginViewModel Input { get; set; }

	public string ErrorMessage { get; set; }

	public void OnGet()
	{
		_logger.LogInformation("OnGet method called."); // Ghi log khi OnGet được gọi
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == Input.Email);

		if (user == null || user.Password != Input.Password) // So sánh mật khẩu thô
		{
			ErrorMessage = "Invalid login attempt.";
			return Page();
		}

		// Tạo JWT Token
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty); // Lấy key từ appsettings.json
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] {
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role, user.RoleId), // Đảm bảo RoleId là role trong User model
				new Claim("userId", user.UserId.ToString()), // Thêm claim userId
			}),
			Expires = DateTime.UtcNow.AddHours(1),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		var jwtToken = tokenHandler.WriteToken(token);

		// Lưu token vào Cookie
		HttpContext.Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions
		{
			HttpOnly = true, // Bảo mật chống lại JavaScript
			Expires = DateTime.UtcNow.AddHours(1) // Token hết hạn sau 1 giờ
		});

		// Kiểm tra role của người dùng
		if (user.RoleId == "admin")
		{
			return RedirectToPage("/Shared/Admin/AdminHome"); // Chuyển hướng đến trang AdminHome nếu là admin
		}
		else if (user.RoleId == "user")
		{
			return RedirectToPage("/Shared/Customer/CustomerHome"); // Chuyển hướng đến trang CustomerHome nếu là user
		}

		// Nếu không xác định được role, trả về trang hiện tại với lỗi
		ErrorMessage = "User role not recognized.";
		return Page();
	}
}


