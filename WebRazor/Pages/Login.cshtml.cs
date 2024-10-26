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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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

		// Tạo danh sách các claim
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Email),
			new Claim(ClaimTypes.Role, user.RoleId), // Gán role từ cơ sở dữ liệu vào claim
			new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
		};

        // Tạo identity và principal từ các claim
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Đăng nhập người dùng
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        _logger.LogInformation("Claims: " + string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}")));

        // Log kiểm tra claims sau khi đăng nhập
        var loggedInUser = HttpContext.User;
        _logger.LogInformation("Is User Authenticated After Login: " + loggedInUser.Identity.IsAuthenticated);
        _logger.LogInformation("Claims Count After Login: " + loggedInUser.Claims.Count());

        // Tạo JWT Token sử dụng JwtTokenHelper
        var jwtToken = JwtTokenHelper.GenerateJwtToken(user, _configuration);

		// Lưu token vào Cookie
		HttpContext.Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions
		{
			HttpOnly = true, // Bảo mật chống lại JavaScript
			Expires = DateTime.UtcNow.AddHours(1) // Token hết hạn sau 1 giờ
		});

        return user.RoleId == "admin"
            ? RedirectToPage("/Shared/Admin/AdminHome")
            : RedirectToPage("/Shared/Customer/CustomerHome");
    }
}


