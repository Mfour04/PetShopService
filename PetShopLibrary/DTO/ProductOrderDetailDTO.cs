namespace PetShopLibrary.DTO
{
    public class ProductOrderDetailDTO
    {
        public long OrderDetailId { get; set; }

        public long? OrderId { get; set; }

        public long? ProductId { get; set; }

        public virtual ProductOrderDTO? Order { get; set; }

        public virtual ProductDTO? Product { get; set; }
    }
}
