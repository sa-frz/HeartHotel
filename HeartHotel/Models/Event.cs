using System;
using System.Collections.Generic;

namespace HeartHotel.Models;

public partial class Event
{
    public int Id { get; set; }

    public string? MainText { get; set; }

    public string? Address { get; set; }

    public string? Link1 { get; set; }

    public string? Link2 { get; set; }

    public string? Link3 { get; set; }

    public string? Link4 { get; set; }

    public string? Link5 { get; set; }

    public short? AghayanBanovan { get; set; }

    public string? ZirNevis { get; set; }

    public string? Kondactor { get; set; }

    public string? ImageAdr { get; set; }

    public DateTime? RegisterDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public short? IsOk { get; set; }

    public int? OkedUser { get; set; }

    public DateTime? OkedDate { get; set; }

    public int? TelegramMsgId { get; set; }

    public int? RegisterUser { get; set; }

    public string? RegisterIp { get; set; }

    public decimal? Lat { get; set; }

    public decimal? Lon { get; set; }

    public int? UpdateUser { get; set; }

    public int? OrganizerId { get; set; }

    public bool? Ads { get; set; }

    public string? Video { get; set; }

    public short? TypeId { get; set; }

    public DateTime? PublishDateTime { get; set; }

    public string? Description { get; set; }

    public bool? Global { get; set; }

    public bool? SendtoTelegram { get; set; }

    public string? Title { get; set; }

    public int? CatalogId { get; set; }

    public int? SubjectsId { get; set; }

    public virtual ICollection<Archive> Archives { get; set; } = new List<Archive>();

    public virtual Catalog? Catalog { get; set; }

    public virtual ICollection<EventsDetail> EventsDetails { get; set; } = new List<EventsDetail>();

    public virtual ICollection<EventsPerson> EventsPeople { get; set; } = new List<EventsPerson>();

    public virtual ICollection<EventsService> EventsServices { get; set; } = new List<EventsService>();

    public virtual Organizer? Organizer { get; set; }

    public virtual User? RegisterUserNavigation { get; set; }

    public virtual Subject? Subjects { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Time> Times { get; set; } = new List<Time>();
}
