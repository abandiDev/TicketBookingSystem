# Movie Ticket Booking API

This project is a movie ticket booking system built with ASP.NET Core (.NET 8.0) and PostgreSQL. It exposes a RESTful API for booking movie tickets, managing movies, showtimes, and users. The API includes Swagger UI for interactive documentation and uses Entity Framework Core for data access and migrations.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Running the Application](#running-the-application)
  - [Using Docker Compose](#using-docker-compose)
- [Database Migrations & Seeding](#database-migrations--seeding)
- [API Documentation](#api-documentation)
- [License](#license)

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

1.Ensure Docker is running.
2.From the project root, run

`docker-compose up --build`

3.The API container will build from TicketBookingSystem.Api/Dockerfile and map ports 8080 (HTTP) and 443 (HTTPS) to the host. The PostgreSQL container will be available on port 5432.
