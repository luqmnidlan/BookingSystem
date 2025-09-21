# Booking System

A web-based booking system built with ASP.NET Core and Entity Framework Core.

## Features

- Employee management
- Service management
- Booking appointments
- Client management
- Appointment feedback system

## Technology Stack

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server (Development)
- PostgreSQL (Production)
- Bootstrap (Frontend)

## Local Development

1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Run the application: `dotnet run`

## Deployment

This application is configured to run on Render with PostgreSQL database.

## Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName
```

To update the database:
```bash
dotnet ef database update
```
