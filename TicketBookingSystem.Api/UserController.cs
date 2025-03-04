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

    [HttpGet("{userId}/history")]
    public async Task<IActionResult> Get(int userId)
    {
        var reservations = await _showTimeService.GetReservations(userId);
        var reservationDtos = reservations.Select(x => new ReservationDto
        {
            Id = x.Id,
            UserId = x.UserId,
            NumberOfSeats = x.NumberOfSeats,
            Status = x.Status.GetDisplayName(),
        });
        
        return new OkObjectResult(reservationDtos);
    }
}