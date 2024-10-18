using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebRazor.Models;
namespace WebRazor.Pages
{
    public class CartModel : PageModel
    {
        private readonly PayOS _payOS;
        public CartModel(PayOS payOS)
        {
            _payOS = payOS;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostCreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(body.OrderName, 1, (int)body.TotalPrice);
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)body.TotalPrice, body.Description, items, body.cancelUrl, body.returnUrl);

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
