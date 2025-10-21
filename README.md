# Employee Management API

## Summary

This solution provides a RESTful API to manage **Employees**, **Departments**, and **Projects** using **ASP.NET Core (.NET 9)** and **Entity Framework Core (SQL Server)**.  

- Projects receive a unique code from an external `RandomStringGenerator` service.  
- Transactional creation ensures a project cannot exist without its code (Bonus 1).  
- Docker Compose is provided to run the API and SQL Server locally (Bonus 2).  

---
## Prerequisites

### Option 1: Running Locally
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)  
- SQL Server (2019 or later)
- Visual Studio 2022
- `dotnet-ef` tool for migrations:  
```bash
dotnet tool install --global dotnet-ef
```

### Option 2: Running with Docker
- [Docker](https://www.docker.com/)
- Docker Compose

## Installation & Setup

### Option 1: Local Development

1. **Clone the repository**
```bash
git clone https://github.com/kiric18/complytek_task.git
```

2. **Restore NuGet packages**
```bash
dotnet restore
```

3. **Update Connection String**
   Edit `EmployeeManagement.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CompanyManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
4. **Database**
   - When the EmployeeManagement.Api runs, the database will be created automatically if it does not exist, and initial records will be added to the Departments table.

6. **Run the Application**
   - 
```bash
dotnet run --project EmployeeManagement.Api
```

6. **Access Swagger UI**
Navigate to: `https://localhost:5001/swagger`

### Option 2: Docker Deployment

1. **Clone the repository**
```bash
git clone https://github.com/kiric18/complytek_task.git
```

2. **Build and Run with Docker Compose**
```bash
docker-compose up --build
```

3. **Access the API**
- Swagger UI: `http://localhost:5000/swagger`
- API Base URL: `http://localhost:5000/api`

4. **Stop the containers**
```bash
docker-compose down
```

From the repository root, run:
```bash
docker compose up --build
```
- SQL Server will be available at Server=db,1433 for the API.
- The API container will build and run; check logs for the listening URL.
