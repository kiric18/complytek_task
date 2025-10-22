# Employee Management API

## Summary

This solution provides a RESTful API to manage **Employees**, **Departments**, and **Projects** using **ASP.NET Core (.NET 9)** and **Entity Framework Core (SQL Server)**.  

- Projects receive a unique code from an external `RandomStringGenerator` service.  
- Transactional creation ensures a project cannot exist without its code.  
- Docker Compose is provided to run the API and SQL Server locally.  

Follow the instructions below to build and run the application.
---

## Prerequisites
Before you start, make sure you have the following installed:

- [Docker Desktop](https://www.docker.com/)
   - Ensure virtualization is enabled in BIOS/UEFI.
   - On Windows, WSL 2 backend must be enabled.
- [Git](https://git-scm.com/)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (optional for local build without Docker)
- SQL Server 2022 (optional)

## Project Structure
```pgsql
EmployeeManagement.sln
docker-compose.yml
Dockerfile
.env                <-- environment variables (not committed)
EmployeeManagement.Api/      → ASP.NET Core Web API (entry point)
  ├── Program.cs
  ├── appsettings.json
EmployeeManagement.Application/       → Business logic layer
EmployeeManagement.Infrastructure/      → Database and data access
EmployeeManagement.Core/      → Domain entities and shared models
```
---
## Environment Variables
Create a .env file and add the values below as needed (which Docker Compose supports automatically)
```bash
A_PASSWORD=YourStrong@Passw0rd
ASPNETCORE_ENVIRONMENT=Development
```
---
## Run the Application with Docker
### 1. Build and start all services
From the solution root (same folder as docker-compose.yml):
```bash
docker compose up --build
```
This will:
- Pull the official SQL Server image.
- Build your Employee Management API image using its Dockerfile.
- Start both containers and create a shared network.
  
### 2. Verify it’s running
Once started:
- API available at: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- SQL Server accessible at: localhost,1433 (user: sa, password from .env)

### 3. Stop the containers
To stop and remove containers, run:
```bash
docker compose down
```
To stop but keep data volumes:
```bash
docker compose down --volumes=false
```

### 4. View logs (useful for debugging)
```bash
docker compose logs -f
```
Or view logs for a specific service:
```bash
docker compose logs api
```
---
## Run the Application Locally
### 1. Setup SQL Server
- Use LocalDB, SQL Server Express, or a remote SQL instance.
- The database will be created automatically if it does not exist when the **EmployeeManagement.Api** will be run, and initial records will be added to the Departments table.

### 2. Update `appsettings.Development.json`
In `EmployeeManagement.Api`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
### 3. Run the API
Using the CLI:
```bash
cd EmployeeManagement.Api
dotnet run
```
Or from Visual Studio:
- Set ***EmployeeManagement.Api** as the startup project
- Press F5 (Debug) or Ctrl+F5 (Run)
- API available at: [http://localhost:5000/swagger](http://localhost:5000/swagger)
---
## Common Docker Issues
| Problem                         | Fix                                                                                                            |
| ------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| **Virtualization not detected** | Enable virtualization in BIOS or Windows Features (`Windows Subsystem for Linux` + `Virtual Machine Platform`) |
| **SQL Server fails to start**   | Ensure `SA_PASSWORD` meets SQL complexity rules                                                                |
| **ERR_EMPTY_RESPONSE**          | Wait a few seconds after SQL container starts (API depends on DB health check)                                 |
| **Dockerfile not found**        | Verify `Dockerfile` in `docker-compose.yml` and run from the solution root                                     |
---
## Useful Commands
| Command                                       | Description                |
| --------------------------------------------- | -------------------------- |
| `docker compose up --build`                   | Build and run containers   |
| `docker compose down`                         | Stop and remove containers |
| `docker ps`                                   | List running containers    |
| `docker exec -it company-sqlserver /bin/bash` | Enter SQL Server container |
| `dotnet build`                                | Build the solution locally |
| `dotnet run --project EmployeeManagement.Api` | Run API locally            |

