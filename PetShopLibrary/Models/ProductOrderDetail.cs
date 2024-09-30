using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class ProductOrderDetail
{
    public long OrderDetailId { get; set; }

    public long? OrderId { get; set; }

    public long? ProductId { get; set; }

    public virtual ProductOrder? Order { get; set; }

    public virtual Product? Product { get; set; }
}
