using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EventsService> EventsServices { get; set; } = new List<EventsService>();
}
