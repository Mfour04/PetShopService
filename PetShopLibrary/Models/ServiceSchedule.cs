using System;
using System.Collections.Generic;

namespace PetShopLibrary.Models;

public partial class ServiceSchedule
{
    public long ScheduleId { get; set; }

    public long? UserId { get; set; }

    public long? ServiceId { get; set; }

    public DateTime? Date { get; set; }

    public long? Status { get; set; }

    public virtual ShopService? Service { get; set; }

    public virtual User? User { get; set; }
}
