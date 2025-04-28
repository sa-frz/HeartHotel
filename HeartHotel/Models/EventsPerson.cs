using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class EventsPerson
{
    public int Id { get; set; }

    public int? EventsId { get; set; }

    public int? PersonId { get; set; }

    /// <summary>
    /// 1: سخنران
    /// 2: مداح
    /// 3: مجری
    /// 4: اجرای دکلمه
    /// 5: قرآن
    /// 6: مهمان
    /// 7: کارشناس
    /// 8: دعا
    /// 9: پیش منبر
    /// </summary>
    public short? Type { get; set; }

    public virtual Event? Events { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual Person? Person { get; set; }
}
