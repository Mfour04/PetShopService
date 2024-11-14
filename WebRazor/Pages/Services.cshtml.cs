using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Service;

namespace WebRazor.Pages
{
    public class ServicesModel : PageModel
    {
        private readonly ShopServiceService _shopServiceService;

        public ServicesModel(ShopServiceService shopServiceService)
        {
            _shopServiceService = shopServiceService;
        }

        public List<ShopService> Services { get; set; } = new List<ShopService>();

        public async Task OnGetAsync()
        {
            Services = (await _shopServiceService.GetAllServicesAsync()).ToList();
        }
    }
}
