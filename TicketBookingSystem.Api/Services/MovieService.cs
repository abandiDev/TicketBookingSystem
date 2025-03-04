using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;
using TicketBookingSystem.Repositories;

namespace TicketBookingSystem.Services;

public class MovieService : IMovieService
{
    private readonly TicketBookingDbContext _dbContext;

    public MovieService(TicketBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Movie>> Register(RegisterMovieDto dto)
    {
        try
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
            };

            foreach (var showtime in dto.Showtimes)
            {
                movie.AddShowtime(new Showtime
                {
                    Movie = movie,
                    StartTime = showtime.StartTime,
                    AvailableSeats = showtime.SeatingCapacity,
                    SeatingCapacity = showtime.SeatingCapacity
                });
            }

            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result<Movie>.Success(movie);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}