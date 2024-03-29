﻿// <auto-generated />
using System;
using AquaparkWebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AquaparkWebApplication1.Migrations
{
    [DbContext(typeof(AquaparkDbContext))]
    [Migration("20230305200136_DeleteBehaviourChangeMigration")]
    partial class DeleteBehaviourChangeMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AquaparkWebApplication1.Models.Hall", b =>
                {
                    b.Property<byte>("HallId")
                        .HasColumnType("tinyint")
                        .HasColumnName("HallID");

                    b.Property<byte>("HallMaxPeople")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("PoolsMaxDepth")
                        .HasColumnType("decimal(2, 1)");

                    b.Property<decimal>("PoolsMinDepth")
                        .HasColumnType("decimal(2, 1)");

                    b.HasKey("HallId");

                    b.ToTable("Hall", (string)null);
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Pool", b =>
                {
                    b.Property<byte>("PoolId")
                        .HasColumnType("tinyint")
                        .HasColumnName("PoolID");

                    b.Property<byte?>("Hall")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("PoolDepth")
                        .HasColumnType("decimal(2, 1)");

                    b.Property<byte?>("PoolMinHeight")
                        .HasColumnType("tinyint");

                    b.Property<byte>("WaterType")
                        .HasColumnType("tinyint");

                    b.HasKey("PoolId");

                    b.HasIndex("Hall");

                    b.ToTable("Pool", (string)null);
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Slide", b =>
                {
                    b.Property<byte>("SlideId")
                        .HasColumnType("tinyint")
                        .HasColumnName("SlideID");

                    b.Property<byte>("SlideHighestPoint")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("SlideMaxHeight")
                        .HasColumnType("tinyint");

                    b.Property<byte>("SlideMaxPeople")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("SlideMaxWeight")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("SlideMinAge")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("SlideMinHeight")
                        .HasColumnType("tinyint");

                    b.Property<string>("SlideName")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.HasKey("SlideId");

                    b.ToTable("Slide", (string)null);
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .HasColumnType("int")
                        .HasColumnName("TicketID");

                    b.Property<byte?>("LocationHall")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("LocationId")
                        .HasColumnType("tinyint")
                        .HasColumnName("LocationID");

                    b.Property<byte?>("LocationSlide")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("Price")
                        .HasColumnType("smallmoney");

                    b.Property<int?>("TicketOwner")
                        .HasColumnType("int");

                    b.Property<byte>("TicketStatus")
                        .HasColumnType("tinyint");

                    b.HasKey("TicketId");

                    b.HasIndex("LocationId");

                    b.HasIndex("TicketOwner");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Visitor", b =>
                {
                    b.Property<int>("VisitorId")
                        .HasColumnType("int")
                        .HasColumnName("VisitorID");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date");

                    b.Property<byte>("Height")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Weight")
                        .HasColumnType("tinyint");

                    b.HasKey("VisitorId");

                    b.ToTable("Visitor", (string)null);
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Pool", b =>
                {
                    b.HasOne("AquaparkWebApplication1.Models.Hall", "HallNavigation")
                        .WithMany("Pools")
                        .HasForeignKey("Hall")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Pool_Hall");

                    b.Navigation("HallNavigation");
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Ticket", b =>
                {
                    b.HasOne("AquaparkWebApplication1.Models.Hall", "Location")
                        .WithMany("Tickets")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Ticket_Hall");

                    b.HasOne("AquaparkWebApplication1.Models.Slide", "LocationNavigation")
                        .WithMany("Tickets")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Ticket_Slide");

                    b.HasOne("AquaparkWebApplication1.Models.Visitor", "TicketOwnerNavigation")
                        .WithMany("Tickets")
                        .HasForeignKey("TicketOwner")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Ticket_Visitor");

                    b.Navigation("Location");

                    b.Navigation("LocationNavigation");

                    b.Navigation("TicketOwnerNavigation");
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Hall", b =>
                {
                    b.Navigation("Pools");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Slide", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("AquaparkWebApplication1.Models.Visitor", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
