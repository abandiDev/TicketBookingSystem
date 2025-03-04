using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;

namespace TicketBookingSystem.Services;

public interface IShowTimeService
{
    Task<Result<Reservation>> Book(int showTimeId, BookShowDto dto);
    Task<Result> Cancel(int showTimeId, int reservationId);
    Task<Result<Reservation>> GetReservation(int showTimeId, int reservationId);
}