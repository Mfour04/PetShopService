namespace PetShopLibrary.DTO
{
    public class ProductDTO
    {
        public long ProductId { get; set; }

        public long? CategoryId { get; set; }

        public string? ProductName { get; set; }

        public long? Price { get; set; }

        public long? Status { get; set; }

        public string? Description { get; set; }

        public string? FilePath { get; set; }

        public virtual ProductCategoryDTO? Category { get; set; }
    }
}
