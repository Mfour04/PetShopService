namespace PetShopLibrary.DTO
{
    public class UserDTO
    {
        public long UserId { get; set; }

        public string? RoleId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool? Gender { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<ProductOrderDTO>? ProductOrders { get; set; } = new List<ProductOrderDTO>();
    }
}
