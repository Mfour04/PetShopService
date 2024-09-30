using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? RoleId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool? Gender { get; set; }

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

    public virtual ICollection<ServiceSchedule> ServiceSchedules { get; set; } = new List<ServiceSchedule>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
