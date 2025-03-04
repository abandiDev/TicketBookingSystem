using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TicketBookingSystem.Repositories;

public class TicketBookingDbContextFactory : IDesignTimeDbContextFactory<TicketBookingDbContext>
{
    public TicketBookingDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.json (or other sources).
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<TicketBookingDbContext>();

        // Use your relational database connection string here.
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connectionString);

        return new TicketBookingDbContext(builder.Options);
    }
}