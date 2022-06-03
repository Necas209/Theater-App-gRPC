﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(TheaterDbContext))]
    partial class TheaterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GrpcLibrary.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Funds")
                        .HasPrecision(2)
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Stamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Movement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Stamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasPrecision(2)
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("NoTickets")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfPurchase")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Total")
                        .HasPrecision(2)
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SessionId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Length")
                        .HasColumnType("time");

                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Showtime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TheaterId")
                        .HasColumnType("int");

                    b.Property<decimal>("TicketPrice")
                        .HasPrecision(2)
                        .HasColumnType("money");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShowId");

                    b.HasIndex("TheaterId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Show", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Theater", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Theaters");
                });

            modelBuilder.Entity("GrpcLibrary.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Watched", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.HasKey("ClientId", "ShowId");

                    b.HasIndex("ShowId");

                    b.ToTable("Watched");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Admin", b =>
                {
                    b.HasOne("GrpcLibrary.Models.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("GrpcLibrary.Models.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Client", b =>
                {
                    b.HasOne("GrpcLibrary.Models.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("GrpcLibrary.Models.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Log", b =>
                {
                    b.HasOne("GrpcLibrary.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Manager", b =>
                {
                    b.HasOne("GrpcLibrary.Models.User", "User")
                        .WithOne("Manager")
                        .HasForeignKey("GrpcLibrary.Models.Manager", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Movement", b =>
                {
                    b.HasOne("GrpcLibrary.Models.Client", "Client")
                        .WithMany("Movements")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Reservation", b =>
                {
                    b.HasOne("GrpcLibrary.Models.Client", "Client")
                        .WithMany("Reservations")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrpcLibrary.Models.Session", "Session")
                        .WithMany("Reservations")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Session", b =>
                {
                    b.HasOne("GrpcLibrary.Models.Show", "Show")
                        .WithMany("Sessions")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrpcLibrary.Models.Theater", "Theater")
                        .WithMany("Sessions")
                        .HasForeignKey("TheaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Show");

                    b.Navigation("Theater");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Show", b =>
                {
                    b.HasOne("GrpcLibrary.Models.Genre", "Genre")
                        .WithMany("Shows")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Watched", b =>
                {
                    b.HasOne("GrpcLibrary.Models.Client", "Client")
                        .WithMany("ShowsWatched")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrpcLibrary.Models.Show", "Show")
                        .WithMany("ClientsWatched")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Client", b =>
                {
                    b.Navigation("Movements");

                    b.Navigation("Reservations");

                    b.Navigation("ShowsWatched");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Genre", b =>
                {
                    b.Navigation("Shows");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Session", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Show", b =>
                {
                    b.Navigation("ClientsWatched");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("GrpcLibrary.Models.Theater", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("GrpcLibrary.Models.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Client");

                    b.Navigation("Logs");

                    b.Navigation("Manager");
                });
#pragma warning restore 612, 618
        }
    }
}
