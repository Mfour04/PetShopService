using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.Diagnostics;

namespace WebRazor.Pages.Admin
{
    public class TableModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProductCategoryService _productCategoryService;
        public TableModel(UserService userService, ProductService productService, ProductCategoryService productCategoryService)
        {
            _userService = userService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IEnumerable<User> Users { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public User user { get; set; }
        public Product product { get; set; }
        public async Task OnGetAsync(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                Users = _userService.SearchUsers(keyword);
            } else
            {
                Users = _userService.GetUsers();
                Products = _productService.GetAllProducts();
            }
        }

        public async Task<IActionResult> OnGetEditProductsAsync(long id)
        {
            var product = _productService.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_productCategoryService.GetProductCategories(), "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostEditProductAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _productService.AddProduct(product);
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _userService.UpdateUser(user);
                var users = _userService.GetUsers();
                return RedirectToPage(); 
            }
            return Page(); 
        }

        public async Task<IActionResult> OnPostDeleteAsync(int userId)
        {
            if (userId <= 0)
            {
                return NotFound(); 
            }

            var user = _userService.GetUserById(userId);
            if (user != null)
            {
                _userService.Delete(userId);
                Users = _userService.GetUsers();
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage();
        }

    }
}
