namespace WebRazor.Models
{
    public class CreatePaymentLinkRequest
    {
        public string productName {  get; set; }
        //show about "userName" payment
        public string description {  get; set; }
        public int price {  get; set; }
        public string returnUrl { get; set; } = "https://localhost:44376/Index";
        public string cancelUrl { get; set; } = "https://localhost:44376/Shop";
    }
}
