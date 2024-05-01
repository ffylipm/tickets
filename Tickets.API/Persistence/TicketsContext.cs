using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tickets.Persistence;

public partial class TicketsContext : DbContext
{
    public TicketsContext()
    {
    }

    public TicketsContext(DbContextOptions<TicketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolMenu> RolMenus { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=18.221.84.173;Database=Tickets; User Id=ws;Password=ws; Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__2DC7BD0989879D0E");

            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.MaxTicketQty).HasColumnName("maxTicketQty");
            entity.Property(e => e.MinTicketQty).HasColumnName("minTicketQty");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PlaceId).HasColumnName("placeId");

            entity.HasOne(d => d.Place).WithMany(p => p.Events)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_placeId");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menu__3B4071746C1135FA");

            entity.ToTable("Menu");

            entity.Property(e => e.MenuId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("menuId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("path");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Place__E1216A361CF0E650");

            entity.ToTable("Place");

            entity.Property(e => e.PlaceId).HasColumnName("placeId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.NameFull)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("nameFull");
            entity.Property(e => e.NameShort)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nameShort");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__540236348EF534FE");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rolId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Virtual).HasColumnName("virtual");
        });

        modelBuilder.Entity<RolMenu>(entity =>
        {
            entity.HasKey(e => new { e.MenuId, e.RolId }).HasName("PK_RolMenu_menuId_rolId");

            entity.ToTable("RolMenu");

            entity.Property(e => e.MenuId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("menuId");
            entity.Property(e => e.RolId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rolId");
            entity.Property(e => e.Active).HasColumnName("active");

            entity.HasOne(d => d.Menu).WithMany(p => p.RolMenus)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolMenu_menuId");

            entity.HasOne(d => d.Rol).WithMany(p => p.RolMenus)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolMenu_rolId");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__3333C61045FA78F0");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticketId");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.IssueOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("issueOn");
            entity.Property(e => e.Used).HasColumnName("used");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("userId");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ticket_eventId");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ticket_userId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CFFE838AC71");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("userId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Document)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("document");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("documentType");
            entity.Property(e => e.Lastname)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Name)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<UserRol>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RolId }).HasName("PK_userId_rolId");

            entity.ToTable("UserRol");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("userId");
            entity.Property(e => e.RolId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rolId");
            entity.Property(e => e.Active).HasColumnName("active");

            entity.HasOne(d => d.Rol).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userRol_rolId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userRol_userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
