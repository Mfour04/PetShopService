using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.Security.Claims;

namespace WebRazor.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserService _userService;

		public ProfileModel(UserService userService)
		{
			_userService = userService;
		}

		[BindProperty]
        public User Users { get; set; }

        public async Task<IActionResult> OnGetAsync(long userId)
        {
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if(userIdClaim == null)
            {
                return Page();
            }
            userId = long.Parse(userIdClaim.Value);
			Users = _userService.GetUserById(userId);

            if (Users == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
