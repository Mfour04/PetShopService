using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class Transaction
{
    public long TransactionId { get; set; }

    public long? UserId { get; set; }

    public DateTime? Date { get; set; }

    public long? Price { get; set; }

    public virtual User? User { get; set; }
}
