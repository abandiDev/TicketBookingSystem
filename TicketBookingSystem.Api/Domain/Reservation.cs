using System.ComponentModel;

namespace TicketBookingSystem.Domain;

public enum ReservationStatus
{
    Active,
    Cancelled,
}

public class Reservation
{
    public int Id { get; set; }
    public Showtime Showtime { get; set; }
    public int ShowtimeId { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public int NumberOfSeats { get; set; }
    public DateTime BookingTime { get; set; }
    public ReservationStatus Status { get; set; }
}