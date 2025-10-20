using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Settings;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add user secrets
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RandomOrgSettings>(
    builder.Configuration.GetSection("RandomStringApi")
);

builder.Services.AddHttpClient<IRandomStringGenerator, RandomStringGenerator>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
