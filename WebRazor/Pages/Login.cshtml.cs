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

	private readonly ILogger<LoginModel> _logger;

	private readonly IConfiguration _configuration;

	public LoginModel(PetShopContext context, ILogger<LoginModel> logger, IConfiguration configuration)
	{
		_context = context;
		_logger = logger;
		_configuration = configuration;
	}

	[BindProperty]
	public LoginViewModel Input { get; set; }

	public string ErrorMessage { get; set; }

	[BindProperty(SupportsGet = true)]
	public string ReturnUrl { get; set; }

	public void OnGet()
	{
		_logger.LogInformation("OnGet method called."); 
	}

	public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
	{
		returnUrl ??= ReturnUrl ?? "/"; // Nếu không có returnUrl, mặc định về trang chủ

		//if (!ModelState.IsValid)
		//{
		//	return Page();
		//}

		var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == Input.Email);

		if (user == null || user.Password != Input.Password)
		{
			ErrorMessage = "Invalid login attempt.";
			return Page();
		}

		if (!user.IsEmailVerified)
		{
			ErrorMessage = "Email chưa được xác thực. Vui lòng kiểm tra email của bạn.";
			return Page();
		}

		var claims = new List<Claim>
	 {
		  new Claim(ClaimTypes.Name, user.Email),
		  new Claim(ClaimTypes.Role, user.RoleId),
		  new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
	 };

		var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
		{
			IsPersistent = true,
			ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
		});

		_logger.LogInformation("User logged in successfully.");

		// Nếu là admin -> vào Dashboard
		if (user.RoleId == "admin")
		{
			return RedirectToPage("/Admin/Dashboard");
		}

		// Nếu là user -> về returnUrl
		return LocalRedirect(returnUrl);
	}
}


