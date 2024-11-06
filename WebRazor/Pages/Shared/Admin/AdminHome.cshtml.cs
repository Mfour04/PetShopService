using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;

namespace WebRazor.Pages.Shared.Admin
{
    [Authorize(Roles = "admin")]  // Chỉ cho phép admin truy cập
    public class AdminHomeModel : PageModel
    {
		private readonly ILogger<AdminHomeModel> _logger; // Tạo biến logger
        private readonly IUserRepository _userRepository;
		private readonly IProductRepository _productRepository;
		private readonly IShopServiceRepository _shopServiceRepository;
		// Inject ILogger vào thông qua constructor
		public AdminHomeModel(ILogger<AdminHomeModel> logger,
			IUserRepository userRepository,
			IProductRepository productRepository,
			IShopServiceRepository shopServiceRepository)
		{
			_logger = logger; // Gán logger
            _userRepository = userRepository;
			_productRepository = productRepository;
			_shopServiceRepository = shopServiceRepository;
		}

        public IEnumerable<User> Users { get; set; }
		public IEnumerable<Product> Products { get; set; }

		public IEnumerable<ShopService> ShopServices { get; set; }

		public async Task<IActionResult> OnGet()
        {
            // Ghi log các claim của người dùng
            var userClaims = User.Claims.ToList();
			Users = _userRepository.GetUsers();
			Products = _productRepository.GetAllProducts();
			ShopServices = _shopServiceRepository.GetShopServices();
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

		public JsonResult OnGetUsers()
		{
			var users = _userRepository.GetUsers();
			return new JsonResult(users);
		}

		public JsonResult OnGetProducts()
		{
			var products = _productRepository.GetAllProducts();
			return new JsonResult(products);
		}

		public JsonResult OnGetShopServices()
		{
			var services = _shopServiceRepository.GetShopServices();
			return new JsonResult(services);
		}

		public async Task<IActionResult> OnPostUpdateUserAsync(User user)
		{
			try
			{
				_userRepository.UpdateUser(user);
			}
			catch (DbUpdateException)
			{
				ModelState.AddModelError("", "Edit Fail");
				return Page();
			}
			return RedirectToPage("/");
		}

		//public async Task<IActionResult> OnPostUpdateProductAsync(Product updatedProduct)
		//{
		//	if (updatedProduct == null || updatedProduct.ProductId <= 0)
		//	{
		//		ModelState.AddModelError("", "Invalid product data.");
		//		return Page();
		//	}

		//	try
		//	{
		//		_productService.UpdateProduct(updatedProduct);
		//		// Tải lại trang để thấy kết quả cập nhật
		//		return Page();
		//	}
		//	catch (Exception ex)
		//	{
		//		ModelState.AddModelError("", $"Error: {ex.Message}");
		//		return Page();
		//	}
		//}
		public JsonResult OnPostGetUserById(long userId)
		{
			var user = _userRepository.GetUserById(userId);
			if (user == null)
			{
				return new JsonResult(null);
			}
			return new JsonResult(user);
		}
	}
}
