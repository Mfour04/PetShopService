using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Service;

namespace WebRazor.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ProductService _productService;
        public ShopModel(ProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<Product> Products { get; set; }

        public void OnGet()
        {
            Products = _productService.GetAllProducts();
        }
    }
}
