using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class TicketsSold
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public string Code { get; set; } = null!;

    public int Type { get; set; }

    public DateTime Date { get; set; }

    public string CodeDigit { get; set; } = null!;

    public bool? Certificate { get; set; }

    public virtual Payment Payment { get; set; } = null!;

    public virtual ICollection<TicketsSoldService> TicketsSoldServices { get; set; } = new List<TicketsSoldService>();

    public virtual ICollection<Traffic> Traffics { get; set; } = new List<Traffic>();
}
