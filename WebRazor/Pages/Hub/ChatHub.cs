using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        // Lấy UserId và UserName từ các claim
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Guest";

        // Gửi tin nhắn đến tất cả các client cùng với UserId và UserName
        await Clients.All.SendAsync("ReceiveMessage", userId, userName, message);

        // Gửi thông báo đến admin nếu có tin nhắn mới
        if (userId != "AdminId") // Giả sử admin có UserId là "AdminId"
        {
            await Clients.User("AdminId").SendAsync("NewMessageNotification", userName, message);
        }
    }

    public async Task SendMessageToUser(string userId, string message)
    {
        // Admin gửi tin nhắn đến một user cụ thể
        var adminName = "Admin"; // Tên hiển thị của admin
        await Clients.User(userId).SendAsync("ReceiveMessage", "AdminId", adminName, message);
    }
}