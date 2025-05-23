using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class VenueHall
{
    public int Id { get; set; }

    public int VenueId { get; set; }

    public string Title { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual ICollection<Programs> Programs { get; set; } = new List<Programs>();

    public virtual Venue Venue { get; set; } = null!;

    public virtual ICollection<VenueHallManager> VenueHallManagers { get; set; } = new List<VenueHallManager>();

    public virtual ICollection<VenueHallMonitor> VenueHallMonitors { get; set; } = new List<VenueHallMonitor>();
}
