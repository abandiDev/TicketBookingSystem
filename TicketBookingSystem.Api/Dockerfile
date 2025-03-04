# Build and publish stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file to leverage Docker layer caching
COPY TicketBookingSystem.Api/TicketBookingSystem.Api.csproj TicketBookingSystem.Api/
RUN dotnet restore TicketBookingSystem.Api/TicketBookingSystem.Api.csproj

# Copy the entire source and publish the app in Debug configuration.
COPY . .
WORKDIR /src/TicketBookingSystem.Api
RUN dotnet publish -c Debug -o /app/publish --no-restore

# Final stage using the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose the desired ports.
EXPOSE 8080
EXPOSE 443

ENTRYPOINT ["dotnet", "TicketBookingSystem.Api.dll"]