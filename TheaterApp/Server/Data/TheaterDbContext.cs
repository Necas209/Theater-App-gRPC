using Microsoft.EntityFrameworkCore;
using GrpcLibrary.Models;

namespace Server.Data;

public class TheaterDbContext: DbContext
{
    public TheaterDbContext(DbContextOptions<TheaterDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Client> Clients { get; set; } = null!;

    public DbSet<Manager> Managers { get; set; } = null!;

    public DbSet<Admin> Admins { get; set; } = null!;

    public DbSet<Theater> Theaters { get; set; } = null!;

    public DbSet<Show> Shows { get; set; } = null!;

    public DbSet<Genre> Genres { get; set; } = null!;

    public DbSet<Session> Sessions { get; set; } = null!;

    public DbSet<Reservation> Reservations { get; set; } = null!;

    public DbSet<Watched> Watched { get; set; } = null!;

    public DbSet<Log> Logs { get; set; } = null!;

    public DbSet<Movement> Movements { get; set; } = null!;

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