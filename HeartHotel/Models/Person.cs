using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Person
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public string? Cv { get; set; }

    public virtual ICollection<EventsPerson> EventsPeople { get; set; } = new List<EventsPerson>();
}
