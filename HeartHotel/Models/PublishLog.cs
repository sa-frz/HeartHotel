using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class PublishLog
{
    public int Id { get; set; }

    public DateTime Published { get; set; }

    public int EventsId { get; set; }

    public short IsOk { get; set; }

    public bool Telegram { get; set; }
}
