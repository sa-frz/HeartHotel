using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class VenueHall
{
    public int Id { get; set; }

    public int VenueId { get; set; }

    public string Title { get; set; } = null!;

    public string? Address { get; set; }

    public virtual Venue Venue { get; set; } = null!;
}
