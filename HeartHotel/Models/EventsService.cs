using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class EventsService
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int ServiceId { get; set; }

    public int Number { get; set; }

    public int Amount { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<PaymentService> PaymentServices { get; set; } = new List<PaymentService>();

    public virtual Service Service { get; set; } = null!;

    public virtual ICollection<TicketsSoldService> TicketsSoldServices { get; set; } = new List<TicketsSoldService>();
}
