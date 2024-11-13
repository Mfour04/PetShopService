using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Chats
{
    public class ChatModel : PageModel
    {
        public string UserId { get; private set; }

        public void OnGet()
        {
            // Lấy UserId từ claim NameIdentifier
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
