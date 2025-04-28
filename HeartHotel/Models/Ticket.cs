using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int EventsId { get; set; }

    public string Title { get; set; } = null!;

    public int Number { get; set; }

    public int Amount { get; set; }

    public virtual Event Events { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
