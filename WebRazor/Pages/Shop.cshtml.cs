using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Service;

namespace WebRazor.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ProductService _productService;
        public PagedResult<Product> PagedProducts { get; set; }
        public int PageSize { get; set; } = 10;

        public ShopModel(ProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<Product> Products { get; set; }

        public async Task OnGetAsync(string? searchText, int pageIndex = 1)
        {
            Products = _productService.GetAllProducts()
                .Where(it =>
                    string.IsNullOrEmpty(searchText) ||
                    it.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            PagedProducts = await _productService.GetPagedProductsAsync(pageIndex, PageSize);
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
