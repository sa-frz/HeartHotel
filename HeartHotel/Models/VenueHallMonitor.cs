using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class VenueHallMonitor
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int HallId { get; set; }

    public int MonitorId { get; set; }

    public virtual VenueHall Hall { get; set; } = null!;
}
