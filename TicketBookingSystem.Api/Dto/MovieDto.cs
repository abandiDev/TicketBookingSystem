public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ShowtimeDto> Showtimes { get; set; }
}