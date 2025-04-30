using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Organizer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public short? EstablishedYear { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerMobile { get; set; }

    public string? Website { get; set; }

    public string? TelegramId { get; set; }

    public string? InstagramId { get; set; }

    public string? AparatId { get; set; }

    public DateTime? RegisterDate { get; set; }

    public string? Address { get; set; }

    public string? Lat { get; set; }

    public string? Lon { get; set; }

    public string? Link1 { get; set; }

    public string? Link2 { get; set; }

    public string? Link3 { get; set; }

    public string? Link4 { get; set; }

    public string? Link5 { get; set; }

    public string? P1 { get; set; }

    public string? P2 { get; set; }

    public string? P3 { get; set; }

    public string? P4 { get; set; }

    public string? P5 { get; set; }

    public string? P6 { get; set; }

    public string? P7 { get; set; }

    public string? P8 { get; set; }

    public string? P9 { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual User User { get; set; } = null!;
}
