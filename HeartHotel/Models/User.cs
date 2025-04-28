using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Password { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Mobile { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? TelegramId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Email { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Organizer> Organizers { get; set; } = new List<Organizer>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Traffic> Traffics { get; set; } = new List<Traffic>();
}
