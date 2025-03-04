using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;

namespace TicketBookingSystem.Services;

public interface IMovieService
{
    Task<Result<Movie>> Register(RegisterMovieDto dto);
}