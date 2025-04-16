using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Venue
{
    public int Id { get; set; }

    public string? Logo { get; set; }

    public string Title { get; set; } = null!;

    public decimal Lat { get; set; }

    public decimal Lon { get; set; }

    public virtual ICollection<VenueHall> VenueHalls { get; set; } = new List<VenueHall>();
}
