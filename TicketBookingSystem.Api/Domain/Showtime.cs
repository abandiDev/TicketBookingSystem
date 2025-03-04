namespace TicketBookingSystem.Domain;

public class Showtime
{
    public int Id { get; set; }
    public Movie Movie { get; set; }
    public int MovieId { get; set; }
    public DateTime StartTime { get; set; }
    public int SeatingCapacity { get; set; }
    public int AvailableSeats { get; set; }
    
    public List<Reservation> Reservations { get; set; } = new();

    public Result<Reservation> Book(int userId, int numberOfSeats)
    {
        if (AvailableSeats - numberOfSeats < 0)
        {
            return Result<Reservation>.Failure("Seats not available.");
        }

        var reservation = new Reservation
        {
            ShowtimeId = Id,
            NumberOfSeats = numberOfSeats,
            UserId = userId,
            BookingTime = DateTime.UtcNow,
            Status = ReservationStatus.Active
        };
        
        AvailableSeats -= numberOfSeats;
        Reservations.Add(reservation);
        return Result<Reservation>.Success(reservation);
    }

    public Result Cancel(int reservationId)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
        {
            return Result.Failure("Reservation not found.");
        }
        
        reservation.Status = ReservationStatus.Cancelled;
        AvailableSeats += reservation.NumberOfSeats;
        return Result.Success();
    }
}