namespace PetShopLibrary.DTO
{
    public class ProductOrderDTO
    {
        public long OrderId { get; set; }

        public long? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public virtual ICollection<ProductOrderDetailDTO> ProductOrderDetails { get; set; } = new List<ProductOrderDetailDTO>();

        public virtual UserDTO? User { get; set; }
    }
}
