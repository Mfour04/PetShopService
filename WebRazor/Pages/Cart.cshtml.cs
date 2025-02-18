using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PetShopLibrary.Service;
using WebRazor.Models;
using PetShopLibrary.Models;
namespace WebRazor.Pages
{
    public class CartModel : PageModel
    {
        private readonly PayOS _payOS;
        private readonly UserService _userService;
        private readonly ProductOrderService _orderService;
        private readonly POrderDetailsService _pOrderService;
        private readonly ProductService _productService;



        public CartModel(PayOS payOS, UserService userService, ProductService productService, ProductOrderService productOrderService, POrderDetailsService pOrderDetail)
        {
            _payOS = payOS;
            _userService = userService;
            _productService = productService;
            _orderService = productOrderService;
            _pOrderService = pOrderDetail;
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
        public async Task<IActionResult> OnPostCreatePaymentLink(CreatePaymentLinkRequest body, long userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            userId = long.Parse(userIdClaim.Value);
            try
            {
                List<long> productId = body.ProductIds.Select(id => Convert.ToInt64(id)).ToList();
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(body.OrderName,productId.Count, (int)body.TotalPrice);
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)body.TotalPrice, body.Description, items, body.cancelUrl, body.returnUrl);

                // add into Order , add rollback
                var orderId = _orderService.OrderProcessing(userId);

                //add into OrderLine
                List<ProductOrderDetail> dtos = new List<ProductOrderDetail>();
                foreach (var pid in productId)
                {
                    ProductOrderDetail it = new ProductOrderDetail
                    {
                        OrderId = orderId,
                        ProductId = pid
                    };
                    dtos.Add(it);
                }
                _pOrderService.AddRangePOrderDetails(dtos);
   


                CreatePaymentResult paymentResult = await _payOS.createPaymentLink(paymentData);

                return Redirect(paymentResult.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Content("Lỗi trong quá trình tạo Payment Link");
            }
        }
    }
}
