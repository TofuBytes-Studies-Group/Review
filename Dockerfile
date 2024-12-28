# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and projects files
COPY *.sln ./

COPY Reviews.API/Reviews.API.csproj Reviews.API/
COPY Reviews.Domain/Reviews.Domain.csproj Reviews.Domain/
COPY Reviews.Infrastructure/Reviews.Infrastructure.csproj Reviews.Infrastructure/
COPY Reviews.UnitTests/Domain.UnitTests.csproj Reviews.UnitTests/
COPY API.Test/API.Test.csproj API.Test/
COPY Infrastructure.Tests/Infrastructure.Tests.csproj Infrastructure.Tests/

# Restore dependencies
RUN dotnet restore 

# Copy the remaining files and build the application
COPY . ./
RUN dotnet publish Reviews.API/Reviews.API.csproj -c Release -o /app/out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Reviews.API.dll"]
