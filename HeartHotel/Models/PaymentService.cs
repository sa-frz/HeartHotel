using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class PaymentService
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public int EventsServiceId { get; set; }

    public int Amount { get; set; }

    public virtual EventsService EventsService { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;
}
