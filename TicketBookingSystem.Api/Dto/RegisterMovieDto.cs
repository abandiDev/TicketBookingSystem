namespace TicketBookingSystem.Dto
{
    public class RegisterMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<RegisterShowtimeDto> Showtimes { get; set; }
    }
}