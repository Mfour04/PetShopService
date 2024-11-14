using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WebRazor.Pages.Admin
{
    public class TableModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProductCategoryService _productCategoryService;
        private readonly IWebHostEnvironment _environment;
        public TableModel(UserService userService, ProductService productService, ProductCategoryService productCategoryService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _environment = environment;
        }
        [BindProperty]
        public IEnumerable<User> Users { get; set; }
        [BindProperty]
        public IEnumerable<Product> Products { get; set; }
        [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public Product product { get; set; }

        [Required(ErrorMessage = "Please choose at least one file.")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }
        public async Task OnGetAsync(string keyword, string searchType)
        {

            if (!string.IsNullOrEmpty(keyword))
            {
                if (searchType == "User")
                {
                    Users = _userService.SearchUsers(keyword);
                    Products = _productService.GetAllProducts();
                }
                else if (searchType == "Product")
                {
                    Products = _productService.SearchProducts(keyword);
                    Users = _userService.GetUsers();
                }
            }
            else
            {
                Users = _userService.GetUsers();
                Products = _productService.GetAllProducts();
                ViewData["CategoryId"] = new SelectList(_productCategoryService.GetProductCategories(), "CategoryId", "CategoryName");
            }         
        }


        public IActionResult OnGetEditProducts(long id)
        {
                Users = _userService.GetUsers();
                Products = _productService.GetAllProducts();
                product = _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                return new JsonResult(product);

        }

            public async Task<IActionResult> OnPostEditProductAsync()
            {
            string savePath = "images/product";
            try
                {
                    string fileName = null;
                    if (FileUploads != null)
                    {
                        foreach (var FileUpload in FileUploads)
                        {
                            fileName = Path.GetFileName(FileUpload.FileName);
                            var file = Path.Combine(_environment.WebRootPath, savePath, FileUpload.FileName);
                        var directoryPath = Path.Combine(_environment.WebRootPath, savePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        using (var fileStream = new FileStream(file, FileMode.Create))
                            {
                                await FileUpload.CopyToAsync(fileStream);
                            }
                        }
                    }
                    Users = _userService.GetUsers();
                    var cateogory = _productCategoryService.GetProductCategoryById(product.Category.CategoryId);
                    var updatedProduct = new Product
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Status = product.Status,
                        Description = product.Description,
                        Category = cateogory,
                        ProductOrderDetails = product.ProductOrderDetails,
                        FilePath = "~/" + savePath + "/" + fileName
                    };
                    _productService.UpdateProduct(updatedProduct);
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
                Products = _productService.GetAllProducts();
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(long productId)
        {
            if (productId <= 0)
            {
                return NotFound();
            }

            var product = _productService.GetProductById(productId);
            if (user != null)
            {
                _productService.DeleteProduct(productId);
                Users = _userService.GetUsers();
                Products = _productService.GetAllProducts();
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostAddProductAsync()
        {
            string savePath = "images/product";
            try
            {
                string fileName = null;
                if (FileUploads != null)
                {
                    foreach (var FileUpload in FileUploads)
                    {
                        fileName = Path.GetFileName(FileUpload.FileName);
                        var file = Path.Combine(_environment.WebRootPath, savePath, FileUpload.FileName);
                        var directoryPath = Path.Combine(_environment.WebRootPath, savePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileStream);
                        }
                    }
                }
                Users = _userService.GetUsers();
                var cateogory = _productCategoryService.GetProductCategoryById(product.Category.CategoryId);
                var updatedProduct = new Product
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Status = product.Status,
                    Description = product.Description,
                    Category = cateogory,
                    ProductOrderDetails = product.ProductOrderDetails,
                    FilePath = "~/" + savePath + "/" + fileName
                };
                _productService.UpdateProduct(updatedProduct);
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return RedirectToPage();
        }
    }
}
