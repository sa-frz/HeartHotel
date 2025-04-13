using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Catalog
{
    public int Id { get; set; }

    public int? TypeId { get; set; }

    public string? Value { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? CreateUser { get; set; }

    public string? Url { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
