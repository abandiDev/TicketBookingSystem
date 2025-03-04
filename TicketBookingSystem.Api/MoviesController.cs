using Microsoft.AspNetCore.Mvc;
using TicketBookingSystem.Dto;
using TicketBookingSystem.Services;

namespace TicketBookingSystem;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : Controller
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
   
    // Register movie with showtimes
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterMovieDto dto)
    {
        var result = await _movieService.Register(dto);
        if (result.IsFailure)
        {
            return new BadRequestObjectResult("Failed to register movie.");
        }
        
        var movie = result.Value;
        return new OkObjectResult(new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Showtimes = movie.Showtimes.Select(x => new ShowtimeDto
            {
                SeatingCapacity = x.SeatingCapacity,
                StartTime = x.StartTime,
                Id = x.Id
            }).ToList(),
        });
    }
}