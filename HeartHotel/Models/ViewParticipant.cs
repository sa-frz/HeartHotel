using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class ViewParticipant
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public string Code { get; set; } = null!;

    public int Type { get; set; }

    public DateTime Date { get; set; }

    public string CodeDigit { get; set; } = null!;

    public DateTime? EntryTime { get; set; }

    public DateTime? OutgoTime { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Title { get; set; }

    public int? EventsId { get; set; }

    public int? UserId { get; set; }

    public string? EventTitle { get; set; }

    public string? Mobile { get; set; }

    public string? Services { get; set; }

    public bool Certificate { get; set; }
}
