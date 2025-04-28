using BusinessWallet.data;
using BusinessWallet.configurations;
using BusinessWallet.repository;
using BusinessWallet.services;
using BusinessWallet.data.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// 1. Database-context registreren (SQLite)
// ---------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(connectionString));   // ← SQLite-provider

// ---------------------------------------------------------------------
// 2. Services registreren
// ---------------------------------------------------------------------
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAutoMapper(typeof(BusinessWallet.utils.MappingProfile));

// ---------------------------------------------------------------------
// 3. API & Swagger
// ---------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------
// 4. Build app
// ---------------------------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------------------------
// 5. Database migraties + seeding
// ---------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    context.Database.Migrate();
    await RoleSeeder.SeedRolesAsync(context);
}

// ---------------------------------------------------------------------
// 6. Middleware-pipeline
// ---------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ---------------------------------------------------------------------
// 7. Demo-endpoint (optioneel)
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

// ---------------------------------------------------------------------
// 8. Start de app
// ---------------------------------------------------------------------
app.Run();

// ---------------------------------------------------------------------
// 9. Record-type voor het demo-endpoint
// ---------------------------------------------------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
