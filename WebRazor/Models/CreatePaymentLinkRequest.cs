namespace WebRazor.Models
{
    public class CreatePaymentLinkRequest
    {
        public string OrderName {  get; set; }
        //show about "userName" payment
        public string Description {  get; set; }
        public long TotalPrice {  get; set; }
        public string returnUrl { get; set; } = "https://localhost:44376/Index";
        public string cancelUrl { get; set; } = "https://localhost:44376/Shop";
    }
}
