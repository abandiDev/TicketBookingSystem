using NUnit.Framework;
using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TicketBookingSystem.Dto;

namespace TicketBookingSystem.Tests;

[TestFixture]
public class MovieControllerTests
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
    public async Task RegisterMovie_WithShowtimesAndSeatingCapacity_ReturnsCreatedMovie()
    {
        var movieRequest = new RegisterMovieDto
        {
            Title = "Test Movie",
            Description = "A sample movie for testing registration.",
            Showtimes = new List<RegisterShowtimeDto>
            {
                new RegisterShowtimeDto
                {
                    StartTime = DateTime.UtcNow.AddHours(2),
                    SeatingCapacity = 100
                },
                new RegisterShowtimeDto
                {
                    StartTime = DateTime.UtcNow.AddHours(4),
                    SeatingCapacity = 150
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(movieRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/movies", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createdMovie = JsonConvert.DeserializeObject<MovieDto>(jsonResponse);
        
        createdMovie.Should().NotBeNull();
        createdMovie.Title.Should().Be("Test Movie");
        createdMovie.Description.Should().Be("A sample movie for testing registration.");
        createdMovie.Showtimes.Should().HaveCount(2);
    }
}