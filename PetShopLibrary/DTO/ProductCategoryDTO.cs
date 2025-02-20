namespace PetShopLibrary.DTO
{
    public class ProductCategoryDTO
    {
        public long CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
