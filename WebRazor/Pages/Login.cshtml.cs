﻿using Microsoft.AspNetCore.Mvc;
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

	//[BindProperty(SupportsGet = true)]
	//public string ReturnUrl { get; set; }

	public void OnGet()
	{
		_logger.LogInformation("OnGet method called."); 
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

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

        // Tạo danh sách các claim
        var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Email),
			new Claim(ClaimTypes.Role, user.RoleId),
			new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
		};

        // Tạo identity và principal từ các claim
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Đăng nhập người dùng qua Cookie Authentication
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
        {
            IsPersistent = true, // Giữ cookie khi đóng trình duyệt nếu cần
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Thời gian hết hạn của cookie
        });

        _logger.LogInformation("User logged in with claims: " + string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}")));

        // Log kiểm tra claims sau khi đăng nhập
        var loggedInUser = HttpContext.User;
        _logger.LogInformation("Is User Authenticated After Login: " + loggedInUser.Identity.IsAuthenticated);
        _logger.LogInformation("Claims Count After Login: " + loggedInUser.Claims.Count());

        //if (Url.IsLocalUrl(ReturnUrl))
        //{
	       // return Redirect(ReturnUrl);
        //}

		return user.RoleId == "admin"
            ? RedirectToPage("/Admin/Dashboard")
            : RedirectToPage("/Index");
    }
}


