using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class ProductCategory
{
    public long CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
