namespace WebRazor.Models.PaymentModel
{
    public class PaymentCallbackRequest
    {
        public string Status { get; set; }   
        public long OrderCode { get; set; }  
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; } 
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}
