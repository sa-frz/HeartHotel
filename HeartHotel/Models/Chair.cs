using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Chair
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Post { get; set; }

    public string? Image { get; set; }

    public string? Cv { get; set; }

    public virtual ICollection<ChairsConductor> ChairsConductors { get; set; } = new List<ChairsConductor>();
}
