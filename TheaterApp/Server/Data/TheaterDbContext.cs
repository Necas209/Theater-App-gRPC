using Microsoft.EntityFrameworkCore;
using GrpcLibrary.Models;

namespace Server.Data;

public class TheaterDbContext: DbContext
{
    public TheaterDbContext(DbContextOptions<TheaterDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Client> Clients { get; set; }
    
    public DbSet<Manager> Managers { get; set; }

    public DbSet<Admin> Admins { get; set; }
    
    public DbSet<Theater> Theaters { get; set; }
    
    public DbSet<Show> Shows { get; set; }
    
    public DbSet<Genre> Genres { get; set; }
    
    public DbSet<Session> Sessions { get; set; }
    
    public DbSet<Reservation> Reservations { get; set; }
    
    public DbSet<Watched> Watched { get; set; }
    
    public DbSet<Log> Logs { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Watched>()
            .HasKey(u => new { u.ClientId, u.ShowId });
        modelBuilder.Entity<Client>()
            .Property(u => u.Funds)
            .HasColumnType("money")
            .HasPrecision(2);
        modelBuilder.Entity<Session>()
            .Property(u => u.TicketPrice)
            .HasColumnType("money")
            .HasPrecision(2);
        modelBuilder.Entity<Transaction>()
            .Property(u => u.Value)
            .HasColumnType("money")
            .HasPrecision(2);
    }
}