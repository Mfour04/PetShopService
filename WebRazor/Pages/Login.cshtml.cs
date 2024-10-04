using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

public class LoginModel : PageModel
{
	private readonly PetShopContext _context;

	public LoginModel(PetShopContext context)
	{
		_context = context;
	}

	[BindProperty]
	public LoginViewModel Input { get; set; }

	public string ErrorMessage { get; set; }

	public void OnGet()
	{
		// Hiển thị trang đăng nhập
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

		// Đăng nhập thành công, xử lý phiên đăng nhập
		// Thêm mã để thiết lập phiên người dùng (như Cookies hoặc JWT token)

		return RedirectToPage("/WelcomePage");
	}
}