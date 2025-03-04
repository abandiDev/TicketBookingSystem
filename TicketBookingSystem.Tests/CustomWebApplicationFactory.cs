using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketBookingSystem.Domain;

namespace TicketBookingSystem.Tests;

using Domain;
using Repositories;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class SeedData
{
    public static void Initialize(TicketBookingDbContext context)
    {
        if (context.Movies.Any())
        {
            return;
        }

        var user1 = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
        var user2 = new User { Id = 2, Name = "Bob", Email = "bob@example.com" };
        context.Users.AddRange(user1, user2);

        var movie = new Movie
        {
            Id = 1,
            Title = "Test Movie",
            Description = "A sample movie for testing purposes."
        };
        
        movie.AddShowtime(new Showtime
        {
            Id = 1,
            StartTime = DateTime.UtcNow.AddHours(1),
            SeatingCapacity = 100,
            AvailableSeats = 100
        });

        movie.AddShowtime(new Showtime
        {
            Id = 2,
            StartTime = DateTime.UtcNow.AddHours(3),
            SeatingCapacity = 150,
            AvailableSeats = 150
        });
        
        context.Movies.Add(movie);
        context.SaveChanges();
    }
}

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TicketBookingDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TicketBookingDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TicketBookingDbContext>();

            db.Database.EnsureCreated();

            SeedData.Initialize(db);
        });
    }
}