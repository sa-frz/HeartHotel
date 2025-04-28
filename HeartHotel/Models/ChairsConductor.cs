using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class ChairsConductor
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    public int ChairId { get; set; }

    public int RoleId { get; set; }

    public virtual Chair Chair { get; set; } = null!;

    public virtual Programs Program { get; set; } = null!;
}
