using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class ProgramConductor
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string SaatAz { get; set; } = null!;

    public int SaatTa { get; set; }

    public virtual Program Program { get; set; } = null!;
}
