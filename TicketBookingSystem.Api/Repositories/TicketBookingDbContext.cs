using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;

namespace TicketBookingSystem.Repositories;

using Microsoft.EntityFrameworkCore;

public class TicketBookingDbContext : DbContext
{
    public TicketBookingDbContext(DbContextOptions<TicketBookingDbContext> options)
        : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PopularShowtimesDto>().HasNoKey();
        modelBuilder.Entity<PeakBookingHoursDto>().HasNoKey();
        
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Showtimes)
            .WithOne(s => s.Movie)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Showtime>()
            .Property(x => x.StartTime)
            .HasColumnType("timestamp with time zone");
        
        modelBuilder.Entity<Showtime>()
            .HasOne(s => s.Movie)
            .WithMany( x=>x.Showtimes)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Showtime>()
            .HasMany(x => x.Reservations)
            .WithOne(x => x.Showtime)
            .HasForeignKey(x => x.ShowtimeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Showtime)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.ShowtimeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(x => x.Reservations) 
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}