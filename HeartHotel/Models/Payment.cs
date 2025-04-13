using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? TicketId { get; set; }

    public int? UserId { get; set; }

    public int? PaymentMethodId { get; set; }

    public DateTime Date { get; set; }

    public int Number { get; set; }

    public int Amount { get; set; }

    public string TrackingCode { get; set; } = null!;

    public bool? IsSuccess { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual ICollection<PaymentService> PaymentServices { get; set; } = new List<PaymentService>();

    public virtual Ticket? Ticket { get; set; }

    public virtual ICollection<TicketsSold> TicketsSolds { get; set; } = new List<TicketsSold>();

    public virtual User? User { get; set; }
}
