using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Monitor
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public int MonitorId { get; set; }
}
