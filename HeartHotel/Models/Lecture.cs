using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Lecture
{
    public int Id { get; set; }

    public int EventsPersonId { get; set; }

    public int TimesId { get; set; }

    public string SaatAz { get; set; } = null!;

    public string SaatTa { get; set; } = null!;

    public string? Text { get; set; }

    public virtual EventsPerson EventsPerson { get; set; } = null!;

    public virtual Time Times { get; set; } = null!;
}
