using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HeartHotel.Models;

public partial class EventisContext : DbContext
{
    public EventisContext()
    {
    }

    public EventisContext(DbContextOptions<EventisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archive> Archives { get; set; }

    public virtual DbSet<Catalog> Catalogs { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventsDetail> EventsDetails { get; set; }

    public virtual DbSet<EventsPerson> EventsPeople { get; set; }

    public virtual DbSet<EventsService> EventsServices { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentService> PaymentServices { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Programs> Programs { get; set; }

    public virtual DbSet<ProgramConductor> ProgramConductors { get; set; }

    public virtual DbSet<PublishLog> PublishLogs { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TempEvent> TempEvents { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketsSold> TicketsSolds { get; set; }

    public virtual DbSet<TicketsSoldService> TicketsSoldServices { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    public virtual DbSet<Traffic> Traffics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    public virtual DbSet<VenueHall> VenueHalls { get; set; }

    public virtual DbSet<ViewParticipant> ViewParticipants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:EventisEntity");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Archive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Archive");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");

            entity.HasOne(d => d.Events).WithMany(p => p.Archives)
                .HasForeignKey(d => d.EventsId)
                .HasConstraintName("FK_Archive_Eventis");
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.ToTable("Catalog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Picture).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Url)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("URL");
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Counter__3214EC27F33A6E60");

            entity.ToTable("Counter");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Dt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DT");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .HasColumnName("IP");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Eventis");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ads).HasDefaultValueSql("('0')");
            entity.Property(e => e.CatalogId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("CatalogID");
            entity.Property(e => e.Description).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Global)
                .IsRequired()
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.ImageAdr).HasMaxLength(255);
            entity.Property(e => e.IsOk).HasColumnName("isOK");
            entity.Property(e => e.Lat).HasColumnType("decimal(20, 16)");
            entity.Property(e => e.Link1).HasMaxLength(255);
            entity.Property(e => e.Link2).HasMaxLength(255);
            entity.Property(e => e.Link3).HasMaxLength(255);
            entity.Property(e => e.Link4).HasMaxLength(255);
            entity.Property(e => e.Link5).HasMaxLength(255);
            entity.Property(e => e.Lon).HasColumnType("decimal(20, 16)");
            entity.Property(e => e.OkedDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("OKedDate");
            entity.Property(e => e.OkedUser).HasColumnName("OKedUser");
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.PublishDateTime)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.RegisterIp)
                .HasMaxLength(22)
                .HasColumnName("RegisterIP");
            entity.Property(e => e.SendtoTelegram)
                .IsRequired()
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.SubjectsId).HasColumnName("SubjectsID");
            entity.Property(e => e.TelegramMsgId).HasColumnName("TelegramMsgID");
            entity.Property(e => e.Title).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.TypeId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("TypeID");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Video)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Catalog).WithMany(p => p.Events)
                .HasForeignKey(d => d.CatalogId)
                .HasConstraintName("FK__Events__CatalogI__08B54D69");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Eventis_Organizer");

            entity.HasOne(d => d.RegisterUserNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.RegisterUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Eventis_Users");

            entity.HasOne(d => d.Subjects).WithMany(p => p.Events)
                .HasForeignKey(d => d.SubjectsId)
                .HasConstraintName("FK_Events_Subjects");
        });

        modelBuilder.Entity<EventsDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");

            entity.HasOne(d => d.Events).WithMany(p => p.EventsDetails)
                .HasForeignKey(d => d.EventsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EventisDetails_Eventis");
        });

        modelBuilder.Entity<EventsPerson>(entity =>
        {
            entity.ToTable("EventsPerson");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.Type).HasComment("1: سخنران\r\n2: مداح\r\n3: مجری\r\n4: اجرای دکلمه\r\n5: قرآن\r\n6: مهمان\r\n7: کارشناس\r\n8: دعا\r\n9: پیش منبر");

            entity.HasOne(d => d.Events).WithMany(p => p.EventsPeople)
                .HasForeignKey(d => d.EventsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EventisPerson_Eventis");

            entity.HasOne(d => d.Person).WithMany(p => p.EventsPeople)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EventisPerson_Person");
        });

        modelBuilder.Entity<EventsService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventsSe__3214EC2764ECDD03");

            entity.ToTable("EventsService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventsServices)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_EventsService_Events");

            entity.HasOne(d => d.Service).WithMany(p => p.EventsServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_EventsService_Services");
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lectures__3214EC27A60F63DC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsPersonId).HasColumnName("EventsPersonID");
            entity.Property(e => e.SaatAz)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SaatTa)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Text).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.TimesId).HasColumnName("TimesID");
            entity.Property(e => e.VenueHallId).HasColumnName("VenueHallID");

            entity.HasOne(d => d.EventsPerson).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.EventsPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lectures_EventsPerson");

            entity.HasOne(d => d.Times).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.TimesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lectures_Times");

            entity.HasOne(d => d.VenueHall).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.VenueHallId)
                .HasConstraintName("FK_Lectures_VenueHalls");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.ToTable("Organizer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.AparatId).HasColumnName("AparatID");
            entity.Property(e => e.InstagramId).HasColumnName("InstagramID");
            entity.Property(e => e.Lat).HasDefaultValue("NULL");
            entity.Property(e => e.Link1).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Link2).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Link3).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Link4).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Link5).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Lon).HasDefaultValue("NULL");
            entity.Property(e => e.OwnerMobile).HasMaxLength(13);
            entity.Property(e => e.P1).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P2).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P3).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P4).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P5).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P6).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P7).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P8).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.P9).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TelegramId).HasColumnName("TelegramID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Organizers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Organizer__UserI__14270015");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OTP__3214EC27504B8EB7");

            entity.ToTable("OTP");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC276AAE58A8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsSuccess)
                .IsRequired()
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Payments__Paymen__58D1301D");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Payments__Ticket__56E8E7AB");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Payments__UserID__57DD0BE4");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC27BFF42CAD");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Url).HasColumnName("URL");
        });

        modelBuilder.Entity<PaymentService>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsServiceId).HasColumnName("EventsServiceID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

            entity.HasOne(d => d.EventsService).WithMany(p => p.PaymentServices)
                .HasForeignKey(d => d.EventsServiceId)
                .HasConstraintName("FK_PaymentServices_EventsService");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentServices)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_PaymentServices_Payments");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cv)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("text")
                .HasColumnName("CV");
            entity.Property(e => e.Image).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Name).HasComment("");
        });

        modelBuilder.Entity<Programs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Programs__3214EC27543800D6");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasMaxLength(10);

            entity.HasOne(d => d.VenueHall).WithMany(p => p.Programs)
                .HasForeignKey(d => d.VenueHallId)
                .HasConstraintName("FK_Programs_VenueHalls");
        });

        modelBuilder.Entity<ProgramConductor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProgramC__3214EC274B7B6484");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.SaatAz)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SaatTa)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramConductors)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK_ProgramConductors_Programs");
        });

        modelBuilder.Entity<PublishLog>(entity =>
        {
            entity.ToTable("PublishLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");
            entity.Property(e => e.IsOk).HasColumnName("isOK");
            entity.Property(e => e.Published).HasColumnType("datetime");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC272F20E26E");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Subjects_Subjects");
        });

        modelBuilder.Entity<TempEvent>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateTime).HasColumnName("Date_Time");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");

            entity.HasOne(d => d.Events).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Events");
        });

        modelBuilder.Entity<TicketsSold>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketsSold");

            entity.ToTable("TicketsSold");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Certificate)
                .IsRequired()
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CodeDigit)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValue("NULL")
                .IsFixedLength();
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Type).HasDefaultValueSql("('0')");

            entity.HasOne(d => d.Payment).WithMany(p => p.TicketsSolds)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketsSold_Payments");
        });

        modelBuilder.Entity<TicketsSoldService>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventsServiceId).HasColumnName("EventsServiceID");
            entity.Property(e => e.TicketsSoldId).HasColumnName("TicketsSoldID");

            entity.HasOne(d => d.EventsService).WithMany(p => p.TicketsSoldServices)
                .HasForeignKey(d => d.EventsServiceId)
                .HasConstraintName("FK_TicketsSoldServices_EventsService");

            entity.HasOne(d => d.TicketsSold).WithMany(p => p.TicketsSoldServices)
                .HasForeignKey(d => d.TicketsSoldId)
                .HasConstraintName("FK_TicketsSoldServices_TicketsSold");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");
            entity.Property(e => e.Rooz).HasMaxLength(10);
            entity.Property(e => e.RoozTa).HasMaxLength(10);
            entity.Property(e => e.SaatAz).HasMaxLength(5);
            entity.Property(e => e.SaatTa).HasMaxLength(5);
            entity.Property(e => e.ShowSaatTa).HasDefaultValueSql("('0')");
            entity.Property(e => e.TelegramNotifMsgId).HasColumnName("TelegramNotifMsgID");

            entity.HasOne(d => d.Events).WithMany(p => p.Times)
                .HasForeignKey(d => d.EventsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Times_Events");
        });

        modelBuilder.Entity<Traffic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Traffics__3214EC2732EF4DAB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TicketsSoldId).HasColumnName("TicketsSoldID");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("UserID");

            entity.HasOne(d => d.TicketsSold).WithMany(p => p.Traffics)
                .HasForeignKey(d => d.TicketsSoldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Traffics__Ticket__7755B73D");

            entity.HasOne(d => d.User).WithMany(p => p.Traffics)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Traffics_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Mobile).HasMaxLength(30);
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Position).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.TelegramId)
                .HasMaxLength(100)
                .HasColumnName("TelegramID");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Venues__3214EC278E34B089");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Lat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Lon).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<VenueHall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VenueHal__3214EC2770528EF8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VenueId).HasColumnName("VenueID");

            entity.HasOne(d => d.Venue).WithMany(p => p.VenueHalls)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("FK_VenueHalls_Venues");
        });

        modelBuilder.Entity<ViewParticipant>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Participants");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CodeDigit)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EntryTime).HasColumnType("datetime");
            entity.Property(e => e.EventsId).HasColumnName("EventsID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Mobile).HasMaxLength(30);
            entity.Property(e => e.OutgoTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
