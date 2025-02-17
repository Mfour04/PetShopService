using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShopLibrary.Models;
using PetShopLibrary.Service;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using PetShopLibrary.DTO;

namespace WebRazor.Pages.Admin
{
    public class TableModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProductCategoryService _productCategoryService;
        private readonly IWebHostEnvironment _environment;
        private readonly ServiceScheduleService _serviceScheduleService;
        public TableModel(UserService userService, ProductService productService, ProductCategoryService productCategoryService, IWebHostEnvironment environment, ServiceScheduleService serviceScheduleService)
        {
            _userService = userService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _environment = environment;
            _serviceScheduleService = serviceScheduleService;
        }
        [BindProperty]
        public IEnumerable<User> Users { get; set; }
        [BindProperty]
        public IEnumerable<Product> Products { get; set; }
        [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public Product product { get; set; }
        [BindProperty]
        public IEnumerable<ServiceSchedule> ServiceSchedules { get; set; }
        [BindProperty]
        public ServiceSchedule serviceSchedule { get; set; }
        [Required(ErrorMessage = "Please choose at least one file.")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile FileUploads { get; set; }
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
                ServiceSchedules = await _serviceScheduleService.GetAllSchedules();
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
                //string fileName = null;
                //if (FileUploads != null)
                //{
                //    foreach (var FileUpload in FileUploads)
                //    {
                //        fileName = Path.GetFileName(FileUpload.FileName);
                //        var file = Path.Combine(_environment.WebRootPath, savePath, FileUpload.FileName);
                //    var directoryPath = Path.Combine(_environment.WebRootPath, savePath);
                //    if (!Directory.Exists(directoryPath))
                //    {
                //        Directory.CreateDirectory(directoryPath);
                //    }
                //    using (var fileStream = new FileStream(file, FileMode.Create))
                //        {
                //            await FileUpload.CopyToAsync(fileStream);
                //        }
                //    }
                //}
                //Users = _userService.GetUsers();
                //var cateogory = _productCategoryService.GetProductCategoryById(product.Category.CategoryId);
                //var updatedProduct = new Product
                //{
                //    ProductId = product.ProductId,
                //    ProductName = product.ProductName,
                //    Price = product.Price,
                //    Status = product.Status,
                //    Description = product.Description,
                //    Category = cateogory,
                //    ProductOrderDetails = product.ProductOrderDetails,
                //    FilePath = "~/" + savePath + "/" + fileName
                //};
                //_productService.UpdateProduct(updatedProduct);
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
            try
            {
                Users = _userService.GetUsers();
                var category = _productCategoryService.GetProductCategoryById(product.Category.CategoryId);
                var newProduct = new ProductDTO
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Status = product.Status,
                    Description = product.Description,
                    CategoryId = category.CategoryId
                };

                await _productService.AddProduct(newProduct, FileUploads);
            }
            catch (Exception ex)
            {
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(long scheduleId, long status)
        {
            var schedule = await _serviceScheduleService.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
            {
                return NotFound();
            }

            await _serviceScheduleService.UpdateScheduleStatusAsync(scheduleId, status);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            user = _userService.GetUserById(long.Parse(userIdClaim));
            if (!string.IsNullOrEmpty(user.Email))
            {
                if (status == 2)
                {
                    await SendEmailAsync(user.Email, schedule.Service?.ServiceName, schedule.Service?.Price, schedule.Date?.ToString("yyyy-MM-dd"));
                }
            }


            ServiceSchedules = await _serviceScheduleService.GetAllSchedules();

            return RedirectToPage();
        }

        private async Task SendEmailAsync(string toEmail, string serviceName, long? price, string date)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("shoppetpath@gmail.com", "dqoympvraxhgjmkr");
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("shoppetpath@gmail.com", "PetPath Team"),
                        Subject = "Thank You for Visiting Our Store",
                        Body = $@"
                    <h2>Thank You for Visiting PetPath!</h2>
                    <p>Dear customer,</p>
                    <p>Thank you for visiting our store! We hope you had a great experience with us.</p>
                    <p><strong>Service:</strong> {serviceName}</p>
                    <p><strong>Price:</strong> ${(price ?? 0):F2}</p>
                    <p><strong>Date of Service:</strong> {date}</p>
                    <p>We truly appreciate your support and look forward to serving you again in the future!</p>
                    <p>Best regards,<br>PetPath Team</p>",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
