using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class TempEvent
{
    public int Id { get; set; }

    public string? Organizer { get; set; }

    public string? Subject { get; set; }

    public string? Title { get; set; }

    public string? Image { get; set; }

    public string? MainText { get; set; }

    public string? Presenter { get; set; }

    public string? Live { get; set; }

    public string? Place { get; set; }

    public string? DateTime { get; set; }

    public string? Zirnevis { get; set; }

    public string? Kondaktor { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }
}
