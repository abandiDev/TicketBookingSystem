using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using TicketBookingSystem.Repositories;
using TicketBookingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movie Ticket Booking API",
        Version = "v1",
        Description = "An API for booking movie tickets"
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// // Configure services.
builder.Services.AddDbContext<TicketBookingDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
//
// // Register business logic services.
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IShowTimeService, ShowTimeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Ticket Booking API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseDeveloperExceptionPage();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketBookingDbContext>();
    if (dbContext.Database.IsNpgsql())
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }
    
    SeedData.Initialize(dbContext);
}
app.Run();

public partial class Program { }