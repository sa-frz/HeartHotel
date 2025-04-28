using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class ChairsConductor
{
    public int Id { get; set; }

    public int ProgramConductorsId { get; set; }

    public int ChairsId { get; set; }

    public virtual Chair Chairs { get; set; } = null!;

    public virtual ProgramConductor ProgramConductors { get; set; } = null!;
}
