using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class TheaterDbContext(DbContextOptions<TheaterDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Client> Clients => Set<Client>();

    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public DbSet<Manager> Managers => Set<Manager>();

    public DbSet<Admin> Admins => Set<Admin>();

    public DbSet<Theater> Theaters => Set<Theater>();

    public DbSet<Show> Shows => Set<Show>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<Watched> Watched => Set<Watched>();

    public DbSet<Log> Logs => Set<Log>();

    public DbSet<Movement> Movements => Set<Movement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Watched>()
            .HasKey(u => new { u.ClientId, u.ShowId });
        modelBuilder.Entity<Client>()
            .Property(u => u.Funds)
            .HasColumnType("money");
        modelBuilder.Entity<Session>()
            .Property(u => u.TicketPrice)
            .HasColumnType("money");
        modelBuilder.Entity<Movement>()
            .Property(u => u.Value)
            .HasColumnType("money");
        modelBuilder.Entity<Reservation>()
            .Property(u => u.Total)
            .HasColumnType("money");
    }
}