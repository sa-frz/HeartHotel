using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Subject
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Subject> InverseParent { get; set; } = new List<Subject>();

    public virtual Subject? Parent { get; set; }
}
