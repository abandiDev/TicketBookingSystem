using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using TicketBookingSystem.Dto;
using TicketBookingSystem.Services;

namespace TicketBookingSystem;

[ApiController]
[Route("api/[controller]")]
public class UserController: Controller
{
    private readonly IShowTimeService _showTimeService;

    public UserController(IShowTimeService showTimeService)
    {
        _showTimeService = showTimeService;
    }

    // Track booking history for users.
    [HttpGet("{userId}/history")]
    public async Task<IActionResult> Get(int userId)
    {
        var reservations = await _showTimeService.GetReservations(userId);
        var reservationDtos = reservations.Select(x => new UserHistoryDto
        {
            NumberOfSeats = x.NumberOfSeats,
            Status = x.Status.GetDisplayName(),
            MovieTitle = x.Showtime.Movie.Title,
            ShowTime = x.Showtime.StartTime
        });
        
        return new OkObjectResult(reservationDtos);
    }
}