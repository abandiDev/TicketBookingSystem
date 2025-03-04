namespace TicketBookingSystem.Domain;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<Showtime> Showtimes { get; set; } = new();

    public void AddShowtime(Showtime showtime)
    {
        Showtimes.Add(showtime);
    }
}