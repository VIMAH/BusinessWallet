using BusinessWallet.data;
using Microsoft.EntityFrameworkCore;
using BusinessWallet.configurations;
using BusinessWallet.repository;
using BusinessWallet.services; 

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// 1. Database-context registreren  (SQLite)
// ---------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(connectionString));   // ← SQLite-provider

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAutoMapper(typeof(BusinessWallet.utils.MappingProfile));


// ---------------------------------------------------------------------
// 2. API & Swagger
// ---------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------------------------------------------------------
// 3. Middleware-pipeline
// ---------------------------------------------------------------------
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapControllers();

// ---------------------------------------------------------------------
// 4. Demo-endpoint (optioneel)
// ---------------------------------------------------------------------
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Urls.Add("http://0.0.0.0:5002");
app.Run();

// ---------------------------------------------------------------------
// 5. Record-type voor het demo-endpoint
// ---------------------------------------------------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
