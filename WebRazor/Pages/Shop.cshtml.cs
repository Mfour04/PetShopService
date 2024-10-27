using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Service;

namespace WebRazor.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly ProductOrderService _orderService;
        public ShopModel(ProductService productService, ProductOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        public IEnumerable<Product> Products { get; set; }

        public void OnGet()
        {
            Products = _productService.GetAllProducts();
        }
        public JsonResult OnPostGetProductById(long productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return new JsonResult(null);
            }
            return new JsonResult(product);
        }
    }
}
