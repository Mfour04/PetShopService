using Microsoft.AspNetCore.Mvc;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using WebRazor.Models.PaymentModel;

namespace WebRazor.Controller
{
    public class PaymentController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ProductOrderService _productOrderService;
        private readonly POrderDetailsService _orderDetailsService;

        public PaymentController(UserService userService,ProductOrderService productOrderService, POrderDetailsService pOrderDetailsService)
        {
            _orderDetailsService = pOrderDetailsService;
            _userService = userService;
            _productOrderService = productOrderService;
        }
        
        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] PaymentCallbackRequest request)
        {
            try
            {
                if (request.Status == "SUCCESS")
                {
                    //var order = new ProductOrder
                    //{
                    //    OrderId = request.OrderCode,
                    //    UserId = request.UserId,
                    //    OrderDate = DateTime.UtcNow,
                    //};

                     _productOrderService.OrderProcessing(request.UserId);

                    return Ok(new { message = "Thanh toán thành công" });
                }

                return BadRequest(new { message = "Thanh toán không thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Lỗi xử lý webhook");
            }
        }
    }
}
