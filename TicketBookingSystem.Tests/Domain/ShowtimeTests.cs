using FluentAssertions;
using NUnit.Framework;
using TicketBookingSystem.Domain;

namespace TicketBookingSystem.Tests.Domain;

[TestFixture]
public class ShowtimeTests
{
    private Showtime _showtime;
    private const int InitialCapacity = 100;

    [SetUp]
    public void SetUp()
    {
        _showtime = new Showtime
        {
            Id = 1,
            MovieId = 1,
            StartTime = DateTime.UtcNow.AddHours(1),
            SeatingCapacity = InitialCapacity,
            AvailableSeats = InitialCapacity
        };
    }

    [Test]
    public void Book_WhenEnoughSeatsAvailable_ShouldReturnSuccessAndDecreaseAvailableSeats()
    {
        var userId = 1;
        var seatsToBook = 10;
        var expectedAvailable = InitialCapacity - seatsToBook;

        var result = _showtime.Book(userId, seatsToBook);

        // Assert using FluentAssertions
        result.IsSuccess.Should().BeTrue("booking should succeed when enough seats are available");
        result.Value.Should().NotBeNull("a reservation should be returned on success");
        _showtime.AvailableSeats.Should().Be(expectedAvailable, "available seats should decrease by the booked amount");
        result.Value.UserId.Should().Be(userId, "the reservation should reflect the correct user");
        result.Value.NumberOfSeats.Should()
            .Be(seatsToBook, "the reservation should have the requested number of seats");
        result.Value.Status.Should().Be(ReservationStatus.Active, "the reservation status should be Active");
        _showtime.Reservations.Should().Contain(result.Value, "the reservation should be added to the showtime list");
    }

    [Test]
    public void Book_WhenNotEnoughSeatsAvailable_ShouldReturnFailureAndNotChangeAvailableSeats()
    {
        var userId = 1;
        var seatsToBook = InitialCapacity + 1; // More than available

        var result = _showtime.Book(userId, seatsToBook);

        result.IsSuccess.Should().BeFalse("booking should fail if insufficient seats are available");
        result.Value.Should().BeNull("no reservation should be returned when booking fails");
        _showtime.AvailableSeats.Should().Be(InitialCapacity, "available seats should remain unchanged");
    }

    [Test]
    public void Cancel_WhenReservationExists_ShouldReturnSuccessAndRestoreAvailableSeats()
    {
        var userId = 2;
        var seatsToBook = 5;
        var bookResult = _showtime.Book(userId, seatsToBook);
        bookResult.IsSuccess.Should().BeTrue("initial booking should succeed");
        var reservation = bookResult.Value;
        var availableAfterBooking = _showtime.AvailableSeats;

        var cancelResult = _showtime.Cancel(reservation.Id);

        cancelResult.IsSuccess.Should().BeTrue("cancellation should succeed for an existing reservation");
        var cancelledReservation = _showtime.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
        cancelledReservation.Should().NotBeNull("reservation should exist in the list");
        cancelledReservation.Status.Should()
            .Be(ReservationStatus.Cancelled, "reservation status should be updated to Cancelled");
        _showtime.AvailableSeats.Should().Be(availableAfterBooking + seatsToBook,
            "available seats should be restored after cancellation");
    }

    [Test]
    public void Cancel_WhenReservationDoesNotExist_ShouldReturnFailure()
    {
        var nonExistentReservationId = 999;
        var cancelResult = _showtime.Cancel(nonExistentReservationId);

        cancelResult.IsSuccess.Should().BeFalse("cancellation should fail for a non-existent reservation");
        cancelResult.Error.Should().Contain("Reservation not found",
            "error message should indicate that the reservation was not found");
    }
}