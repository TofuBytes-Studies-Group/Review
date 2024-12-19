# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and projects files
COPY *.sln .
COPY Reviews.API/Reviews.API.csproj ./Reviews.API/
COPY Reviews.Domain/Reviews.Domain.csproj ./Reviews.Domain/
COPY Reviews.Infrastructure/Reviews.Infrastructure.csproj ./Reviews.Infrastructure/
COPY Reviews.UnitTests/Domain.UnitTests.csproj ./Reviews.UnitTests/
COPY API.Test/API.Test.csproj ./API.Test/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application files
COPY Reviews.API/. ./Reviews.API/
COPY Reviews.Domain/. ./Reviews.Domain/
COPY Reviews.Infrastructure/. ./Reviews.Infrastructure/
COPY Reviews.UnitTests/. ./Reviews.UnitTests/

# Build and publish the API project
WORKDIR /source/Reviews.API
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the build output from the first stage
COPY --from=build /app ./

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Reviews.API.dll"]
