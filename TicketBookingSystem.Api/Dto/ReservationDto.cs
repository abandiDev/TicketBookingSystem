using System.ComponentModel.DataAnnotations;

namespace TicketBookingSystem.Dto;

public class ReservationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ShowtimeId { get; set; }
    public int NumberOfSeats { get; set; }
    public string Status { get; set; }
}