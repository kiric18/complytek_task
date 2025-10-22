# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files
COPY ../EmployeeManagement.Application/EmployeeManagement.Application.csproj EmployeeManagement.Application/
COPY ../EmployeeManagement.Domain/EmployeeManagement.Domain.csproj EmployeeManagement.Domain/
COPY ../EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj EmployeeManagement.Infrastructure/
COPY EmployeeManagement.Api.csproj EmployeeManagement.Api/

# Restore dependencies
RUN dotnet restore EmployeeManagement.Api/EmployeeManagement.Api.csproj

# Copy the rest of the source code
COPY .. .

# Build the app
WORKDIR /src/EmployeeManagement.Api
RUN dotnet build EmployeeManagement.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish EmployeeManagement.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.Api.dll"]
