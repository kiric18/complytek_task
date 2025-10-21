using EmployeeManagement.Api.Filters;
using EmployeeManagement.Api.Middlewares;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add user secrets
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpClient<IRandomStringGenerator, RandomStringGenerator>(client =>
{
    var baseUrl = builder.Configuration["RandomStringApi:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management API", Version = "v1" });
        options.OperationFilter<CustomExceptionResponseFilter>();
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Departments.Any())
        {
            var defaultDepartments = new List<Department>
            {
                new Department { Name = "Human Resources", OfficeLocation = "Building A" },
                new Department { Name = "IT Department", OfficeLocation = "Building B" },
                new Department { Name = "Finance", OfficeLocation = "Building C" },
                new Department { Name = "Marketing", OfficeLocation = "Building D" },
                new Department { Name = "Sales", OfficeLocation = "Building E" },
                new Department { Name = "Customer Support", OfficeLocation = "Building F" },
                new Department { Name = "Research and Development", OfficeLocation = "Innovation Center" },
                new Department { Name = "Operations", OfficeLocation = "Building G" },
                new Department { Name = "Logistics", OfficeLocation = "Warehouse 1" },
                new Department { Name = "Procurement", OfficeLocation = "Building H" },
                new Department { Name = "Legal", OfficeLocation = "Building I" },
                new Department { Name = "Administration", OfficeLocation = "Head Office" },
                new Department { Name = "Quality Assurance", OfficeLocation = "Building J" },
                new Department { Name = "Security", OfficeLocation = "Gate Office" },
                new Department { Name = "Training and Development", OfficeLocation = "Training Center" },
                new Department { Name = "Public Relations", OfficeLocation = "Building K" },
                new Department { Name = "Product Management", OfficeLocation = "Building L" },
                new Department { Name = "Design", OfficeLocation = "Studio 1" },
                new Department { Name = "Engineering", OfficeLocation = "Tech Park" },
                new Department { Name = "Data Analytics", OfficeLocation = "Building M" }
            };

            context.Departments.AddRange(defaultDepartments);
            context.SaveChanges();

            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded default departments successfully.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();