using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public long? CategoryId { get; set; }

    public string? ProductName { get; set; }

    public long? Price { get; set; }

    public long? Status { get; set; }

    public string? Description { get; set; }

    public string? FilePath { get; set; }
        
    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<ProductOrderDetail> ProductOrderDetails { get; set; } = new List<ProductOrderDetail>();
}
