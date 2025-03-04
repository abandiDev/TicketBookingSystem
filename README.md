# Movie Ticket Booking API

This project is a movie ticket booking system built with ASP.NET Core (.NET 8.0) and PostgreSQL. It exposes a RESTful API for registering movies, booking showtimes. The API includes Swagger UI for interactive documentation and uses Entity Framework Core for data access and migrations.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Running the Application](#running-the-application)
  - [Using Docker Compose](#using-docker-compose)
- [Database Migrations & Seeding](#database-migrations--seeding)
- [API Documentation](#api-documentation)

## Features

- **Booking Functionality:** Book seats for movie showtimes .
- **Movie & Showtime Management:** Register movies along with multiple showtimes.
- **Swagger UI:** Interactive API documentation.
- **Docker & Docker Compose:** Easily run the API along with a PostgreSQL database.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

## Running the Application

### Using Docker Compose
A sample docker-compose.yml is provided to run the API along with a PostgreSQL container. 

To run using Docker Compose:

- Ensure Docker is running.
- From the project root, run

`docker-compose up --build`

- The API container will build from TicketBookingSystem.Api/Dockerfile and map ports 8080 (HTTP) and 443 (HTTPS) to the host. The PostgreSQL container will be available on port 5432.

## Database Migrations & Seeding
### Migrations:
Migrations are automatically applied on startup with dbContext.Database.Migrate() in Program.cs.

### Seeding:
Initial data is seeded using the SeedData.Initialize method from TicketBookingSystem.Repositories/SeedData.cs. This inserts sample users (and can be extended to include movies, showtimes, etc.) if the database is empty.

## API Documentation

Swagger UI is enabled by default. When you run the application (via Docker Compose), navigate to:

- http://localhost:8080/index.html (or the mapped port) to view interactive API documentation and test endpoints.