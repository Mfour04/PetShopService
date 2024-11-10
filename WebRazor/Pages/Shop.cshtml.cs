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
        public PagedResult<Product> PagedProducts { get; set; }
        public int PageSize { get; set; } = 10;
        [BindProperty]
        public long MinPrice { get; set; } = 0;
        [BindProperty]
        public long MaxPrice { get; set; } = 5000000;
        public List<Product> FilteredProducts { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public async Task OnGetAsync(string? searchText, int pageIndex = 1, decimal minPrice = 0, decimal maxPrice = 5000000)
        {
            PagedProducts = await _productService.GetPagedProductsAsync(pageIndex, PageSize, searchText, minPrice, maxPrice);
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
