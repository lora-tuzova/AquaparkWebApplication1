using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AquaparkWebApplication1.Models;

public partial class AquaparkDbContext : DbContext
{
    public AquaparkDbContext()
    {
    }

    public AquaparkDbContext(DbContextOptions<AquaparkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<Pool> Pools { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-SN4C7E6\\SQLEXPRESS01;\nDatabase=AquaparkDB; Trusted_Connection=True; Trust Server Certificate = True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hall>(entity =>
        {
            entity.ToTable("Hall");

            entity.Property(e => e.HallId)
                .ValueGeneratedNever()
                .HasColumnName("HallID");
            entity.Property(e => e.PoolsMaxDepth).HasColumnType("decimal(5, 1)");
            entity.Property(e => e.PoolsMinDepth).HasColumnType("decimal(5, 1)");
        });

        modelBuilder.Entity<Pool>(entity =>
        {
            entity.ToTable("Pool");

            entity.Property(e => e.PoolId)
                .ValueGeneratedNever()
                .HasColumnName("PoolID");
            entity.Property(e => e.PoolDepth).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.HallNavigation).WithMany(p => p.Pools)
                .HasForeignKey(d => d.Hall)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pool_Hall");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.ToTable("Slide");

            entity.Property(e => e.SlideId)
                .ValueGeneratedNever()
                .HasColumnName("SlideID");
            entity.Property(e => e.SlideName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId)
                .ValueGeneratedNever()
                .HasColumnName("TicketID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Price).HasColumnType("smallmoney");

            entity.HasOne(d => d.Location).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Hall");

            entity.HasOne(d => d.LocationNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Slide");

            entity.HasOne(d => d.TicketOwnerNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketOwner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Visitor");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.ToTable("Visitor");

            entity.Property(e => e.VisitorId)
                .ValueGeneratedNever()
                .HasColumnName("VisitorID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
