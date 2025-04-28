using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Counter
{
    public int Id { get; set; }

    public string? Ip { get; set; }

    public DateTime? Dt { get; set; }

    public string? Descr { get; set; }

    public string? Agent { get; set; }
}
