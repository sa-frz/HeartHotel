using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Program
{
    public int Id { get; set; }

    public string Date { get; set; } = null!;

    public string ShowDate { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int VenueHallId { get; set; }

    public int ThemeId { get; set; }

    public virtual ICollection<ProgramConductor> ProgramConductors { get; set; } = new List<ProgramConductor>();

    public virtual VenueHall VenueHall { get; set; } = null!;
}
