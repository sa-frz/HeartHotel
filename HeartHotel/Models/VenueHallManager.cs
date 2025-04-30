using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class VenueHallManager
{
    public int Id { get; set; }

    public int VenueHallId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual VenueHall VenueHall { get; set; } = null!;
}
