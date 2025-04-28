using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string? Image { get; set; }

    public string Name { get; set; } = null!;

    public string? Des { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
