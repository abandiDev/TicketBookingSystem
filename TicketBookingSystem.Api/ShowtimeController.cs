using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;
using TicketBookingSystem.Services;

namespace TicketBookingSystem;

[ApiController]
[Route("api/[controller]")]
public class ShowtimeController : Controller
{
    private readonly IShowTimeService _showTimeService;

    public ShowtimeController(IShowTimeService showTimeService)
    {
        _showTimeService = showTimeService;
    }
    
    // Book a showtime
    [HttpPost("{showTimeId}/book")]
    public async Task<IActionResult> Post(int showTimeId, [FromBody] BookShowDto dto)
    {
        var result = await _showTimeService.Book(showTimeId, dto);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var reservation = result.Value;
        return new OkObjectResult(new ReservationDto
        {
            Id = reservation.Id,
            NumberOfSeats =  dto.NumberOfSeats,
            ShowtimeId = showTimeId,
            UserId = dto.UserId,
            Status = reservation.Status.GetDisplayName()
        });
    }

    [HttpPost("{showTimeId}/reservation/{reservationId}/cancel")]
    public async Task<IActionResult> Post(int showTimeId, int reservationId)
    {
        var result = await _showTimeService.Cancel(showTimeId, reservationId);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        
        return Ok();
    }

    
    [HttpGet("{showTimeId}/reservation/{reservationId}")]
    public async Task<IActionResult> GetReservation(int showTimeId, int reservationId)
    {
        var result = await _showTimeService.GetReservation(showTimeId, reservationId);
        if (result.IsFailure)
        {
            return BadRequest("Invalid Reservation");
        }
        
        var reservation = result.Value;
        return new OkObjectResult(new ReservationDto
        {
            Id = reservation.Id,
            NumberOfSeats = reservation.NumberOfSeats,
            ShowtimeId = reservation.ShowtimeId,
            Status =  reservation.Status.GetDisplayName()
        });
    }

    [HttpGet("popular/{topN}")]
    public async Task<IActionResult> GetPopularShows(int topN = 100)
    {
        return Ok(await _showTimeService.GetPopularShows(topN));
    }

    [HttpGet("peakhours/{topN}")]
    public async Task<IActionResult> GetPeakBookingHours(int topN = 100)
    {
        return Ok(await _showTimeService.GetPeakBookingHours(topN));
    }
}