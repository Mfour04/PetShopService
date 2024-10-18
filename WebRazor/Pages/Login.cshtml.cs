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

		// Tạo JWT Token sử dụng JwtTokenHelper
		var jwtToken = JwtTokenHelper.GenerateJwtToken(user, _configuration);

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


