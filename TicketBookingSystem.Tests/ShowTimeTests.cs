using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using TicketBookingSystem.Dto;

namespace TicketBookingSystem.Tests;

public class ShowTimeTests
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    
    [SetUp]
    public void Setup()
    {
        // Initialize the in-memory test server using the application's Startup class.
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
    
    [Test]
    public async Task BookSeats_Success_ReturnsReservation()
    {
        // Arrange
        var bookingRequest = new BookShowDto
        {
            UserId = 1,
            NumberOfSeats = 2
        };

        var content = new StringContent(JsonConvert.SerializeObject(bookingRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/showtime/1/book", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadAsStringAsync();
        var reservation = JsonConvert.DeserializeObject<ReservationDto>(json);
        reservation.Should().NotBeNull();
        reservation.UserId.Should().Be(1);
        reservation.ShowtimeId.Should().Be(1);
        reservation.NumberOfSeats.Should().Be(2);
        reservation.Status.Should().BeEquivalentTo("Active");
    }

    [Test]
    public async Task BookSeats_OverBooking_ReturnsBadRequest()
    {
        var bookingRequest = new
        {
            UserId = 1,
            ShowtimeId = 1,
            NumberOfSeats = 1000
        };

        var content = new StringContent(JsonConvert.SerializeObject(bookingRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/showtime/1/book", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CancelReservation_ValidReservation_ReturnsOk()
    {
        var bookingRequest = new
        {
            UserId = 1,
            ShowtimeId = 1,
            NumberOfSeats = 2
        };

        var content = new StringContent(JsonConvert.SerializeObject(bookingRequest), Encoding.UTF8, "application/json");
        var bookingResponse = await _client.PostAsync("/api/showtime/1/book", content);
        bookingResponse.EnsureSuccessStatusCode();

        var bookingJson = await bookingResponse.Content.ReadAsStringAsync();
        var reservation = JsonConvert.DeserializeObject<ReservationDto>(bookingJson);
        var reservationId = reservation.Id;

        var cancelResponse = await _client.PostAsync($"/api/showtime/1/reservation/{reservationId}/cancel", null);
        cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var reservationResponse = await _client.GetAsync($"api/showtime/1/reservation/{reservationId}");
        reservationResponse.EnsureSuccessStatusCode();
        
        var reservationString = await reservationResponse.Content.ReadAsStringAsync();
        reservation = JsonConvert.DeserializeObject<ReservationDto>(reservationString);
        reservation.Status.Should().BeEquivalentTo("Cancelled");
    }
}