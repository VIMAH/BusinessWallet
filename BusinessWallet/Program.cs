using BusinessWallet.data;
using BusinessWallet.configurations;
using BusinessWallet.repository;
using BusinessWallet.services;
using BusinessWallet.data.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5002");

// ---------------------------------------------------------------------
// 1. Database-context registreren (SQLite)
// ---------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(connectionString));   // ← SQLite-provider

// ---------------------------------------------------------------------
// 2. Repositories registreren
// ---------------------------------------------------------------------
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();

// ---------------------------------------------------------------------
// 3. Services registreren
// ---------------------------------------------------------------------
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
builder.Services.AddAutoMapper(typeof(BusinessWallet.utils.MappingProfile));

// ---------------------------------------------------------------------
// 4. API & Swagger
// ---------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------
// 5. Build app
// ---------------------------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------------------------
// 6. Database migraties + seeding
// ---------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    context.Database.Migrate();
    await RoleSeeder.SeedRolesAsync(context);
}

// ---------------------------------------------------------------------
// 7. Middleware-pipeline
// ---------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ---------------------------------------------------------------------
// 8. Demo-endpoint (optioneel)
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

// app.Urls.Add("http://0.0.0.0:5002");

// ---------------------------------------------------------------------
// 9. Start de app
// ---------------------------------------------------------------------

// ----- Hieronder specifieke tabel data verwijdere, maar niet tabel zelf -----
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//     context.EmployeeRoles.RemoveRange(context.EmployeeRoles);
//     context.Employees.RemoveRange(context.Employees);
//     await context.SaveChangesAsync();
// }

app.Run();

// ---------------------------------------------------------------------
// 10. Record-type voor het demo-endpoint
// ---------------------------------------------------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
