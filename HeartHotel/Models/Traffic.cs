using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Traffic
{
    public int Id { get; set; }

    public int TicketsSoldId { get; set; }

    public DateTime Time { get; set; }

    public int Type { get; set; }

    public int? UserId { get; set; }

    public virtual TicketsSold TicketsSold { get; set; } = null!;

    public virtual User? User { get; set; }
}
