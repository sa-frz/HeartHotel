using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Archive
{
    public int Id { get; set; }

    public int EventsId { get; set; }

    public string? Link { get; set; }

    public string? Description { get; set; }

    public virtual Event Events { get; set; } = null!;
}
