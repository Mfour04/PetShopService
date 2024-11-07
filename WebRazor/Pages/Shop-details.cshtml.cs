using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;

namespace WebRazor.Pages
{
    public class Shop_detailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        public Shop_detailsModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [BindProperty]
        public Product Product { get; set; }
        public  void OnGet(int? id)
        {
            if (id.HasValue)
            {
                var prod = _productRepository.GetProductById((long)id);
                Product = prod;
            }
            else
            {
                Product = null;
            }
        }
    }
}
