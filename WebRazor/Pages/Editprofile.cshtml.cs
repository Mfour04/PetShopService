using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.Security.Claims;

namespace WebRazor.Pages
{
    public class EditprofileModel : PageModel
    {
        private readonly UserService _userService;

		public EditprofileModel(UserService userService)
		{
			_userService = userService;
		}

		[BindProperty]
        public User Users { get; set; } 

        public async Task<IActionResult> OnGetAsync(long userId)
        {

			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return Page();
			}



			var userInDb = _userService.GetUserById(long.Parse(userIdClaim.Value)); 

            if (userInDb == null)
            {
                return NotFound();
            }

            userInDb.Email = Users.Email;
            userInDb.Name = Users.Name;
            userInDb.Address = Users.Address;
            userInDb.Phone = Users.Phone;
            userInDb.Gender = Users.Gender;

            _userService.UpdateUser(userInDb);
            return RedirectToPage("/Profile");
        }
    }
}
