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

    public string SaatTa { get; set; } = null!;

    public virtual ICollection<ChairsConductor> ChairsConductors { get; set; } = new List<ChairsConductor>();

    public virtual Programs Program { get; set; } = null!;
}
