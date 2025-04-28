using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class EventsDetail
{
    public int Id { get; set; }

    public int? EventsId { get; set; }

    public short? NeedBaner { get; set; }

    public short? NeedPersons { get; set; }

    public short? NeedLiveStream { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Event? Events { get; set; }
}
