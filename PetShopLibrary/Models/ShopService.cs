using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class ShopService
{
    public long ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public bool? IsActive { get; set; }

    public long? Price { get; set; }

    public long? Rating { get; set; }

    public virtual ICollection<ServiceSchedule> ServiceSchedules { get; set; } = new List<ServiceSchedule>();
}
