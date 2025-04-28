using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Time
{
    public int Id { get; set; }

    public int EventsId { get; set; }

    public string? Rooz { get; set; }

    public string? SaatAz { get; set; }

    public string? SaatTa { get; set; }

    public short? Roozehafte { get; set; }

    public int? TelegramNotifMsgId { get; set; }

    public string? RoozTa { get; set; }

    public bool? ShowSaatTa { get; set; }

    public string? Description { get; set; }

    public virtual Event Events { get; set; } = null!;

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
