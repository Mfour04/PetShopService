using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class ProductOrder
{
    public long OrderId { get; set; }

    public long? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual ICollection<ProductOrderDetail> ProductOrderDetails { get; set; } = new List<ProductOrderDetail>();

    public virtual User? User { get; set; }
}
