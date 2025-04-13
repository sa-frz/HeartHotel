using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Otp
{
    public int Id { get; set; }

    public string Mobile { get; set; } = null!;

    public DateTime Time { get; set; }

    public int Code { get; set; }
}
