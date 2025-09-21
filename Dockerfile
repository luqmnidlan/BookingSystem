# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookingSystem.csproj", "."]
RUN dotnet restore "BookingSystem.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "BookingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a startup script that runs migrations and starts the app
RUN echo '#!/bin/sh' > /app/start.sh && \
    echo 'dotnet ef database update' >> /app/start.sh && \
    echo 'dotnet BookingSystem.dll' >> /app/start.sh && \
    chmod +x /app/start.sh

ENTRYPOINT ["/app/start.sh"]
