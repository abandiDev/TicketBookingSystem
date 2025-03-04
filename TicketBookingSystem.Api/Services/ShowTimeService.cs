using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Domain;
using TicketBookingSystem.Dto;
using TicketBookingSystem.Repositories;

namespace TicketBookingSystem.Services;

public class ShowTimeService : IShowTimeService
{
    private readonly TicketBookingDbContext _dbContext;

    public ShowTimeService(TicketBookingDbContext  context)
    {
        _dbContext = context;
    }
    
    public async Task<Result<Reservation>> Book(int showTimeId, BookShowDto dto)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var showTime = await _dbContext.Showtimes
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == showTimeId);
        
        if (showTime == null)
        {
            return Result<Reservation>.Failure("Invalid show time");
        }
        
        var result = showTime.Book(dto.UserId , dto.NumberOfSeats);
        if (result.IsFailure)
        {
            return Result<Reservation>.Failure(result.Error);
        }
        
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return result;
    }

    public async Task<Result> Cancel(int showTimeId, int reservationId)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var showTime = await _dbContext.Showtimes
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == showTimeId);
        if (showTime == null)
        {
            return Result<Reservation>.Failure("Invalid show time");
        }
        
        var result = showTime.Cancel(reservationId);
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return result;
    }

    public async Task<Result<Reservation>> GetReservation(int showTimeId, int reservationId)
    {
        var reservation = await _dbContext.Reservations.Include(x => x.Showtime).FirstOrDefaultAsync(x => x.Id == reservationId && x.ShowtimeId == showTimeId);
        if (reservation == null)
        {
            return Result<Reservation>.Failure("Invalid reservation");
        }

        return Result<Reservation>.Success(reservation);
    }

    public async Task<List<Reservation>> GetReservations(int userId)
    {
        return await _dbContext.Reservations.Where( x => x.UserId == userId ).ToListAsync();
    }
}