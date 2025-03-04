using TicketBookingSystem.Domain;

namespace TicketBookingSystem.Repositories;

public static class SeedData
{
    public static void Initialize(TicketBookingDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }
        
        var user1 = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
        var user2 = new User { Id = 2, Name = "Bob", Email = "bob@example.com" };
        
        context.Users.AddRange(user1, user2);
        context.SaveChanges();
    }
}