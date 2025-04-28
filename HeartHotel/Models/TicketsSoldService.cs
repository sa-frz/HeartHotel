using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class TicketsSoldService
{
    public int Id { get; set; }

    public int TicketsSoldId { get; set; }

    public int EventsServiceId { get; set; }

    public virtual EventsService EventsService { get; set; } = null!;

    public virtual TicketsSold TicketsSold { get; set; } = null!;
}
