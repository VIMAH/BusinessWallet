using BusinessWallet.data;
using BusinessWallet.configurations;
using BusinessWallet.repository;
using BusinessWallet.services;
using BusinessWallet.data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BusinessWallet.utils;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPolicyRulesRepository, PolicyRulesRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// ---------------------------------------------------------------------
// 3. Services registreren
// ---------------------------------------------------------------------
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(BusinessWallet.utils.MappingProfile));


// ✅ JWT Utils registreren
var rsaPublic = RSA.Create();
var rsaPrivate = RSA.Create();
// Optioneel: laad keys uit bestanden:
// rsaPublic.ImportFromPem(File.ReadAllText("publickey.pem"));
// rsaPrivate.ImportFromPem(File.ReadAllText("privatekey.pem"));
builder.Services.AddSingleton(new JwtUtils(rsaPublic, rsaPrivate));

// ---------------------------------------------------------------------
// 4. JWT Authentication
// ---------------------------------------------------------------------
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "your-very-long-random-secret-key";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

// ---------------------------------------------------------------------
// 5. API & Swagger
// ---------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BusinessWallet API",
        Version = "v1"
    });

    // ✅ JWT security schema
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ---------------------------------------------------------------------
// 6. CORS policy
// ---------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// ---------------------------------------------------------------------
// 7. Database migraties + seeding
// ---------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    context.Database.Migrate();
    await RoleSeeder.SeedRolesAsync(context);
}

// ---------------------------------------------------------------------
// 8. Middleware-pipeline
// ---------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication(); // ✅ BELANGRIJK: toegevoegd vóór UseAuthorization
app.UseAuthorization();
app.MapControllers();

// ---------------------------------------------------------------------
// 9. Demo-endpoint (optioneel)
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
// 10. Start de app
// ---------------------------------------------------------------------
// -- Optionele cleanup code is hier gelaten maar gecomment
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//     context.EmployeeRoles.RemoveRange(context.EmployeeRoles);
//     context.EmployeeRoleChallenges.RemoveRange(context.EmployeeRoleChallenges);
//     context.Employees.RemoveRange(context.Employees);
//     await context.SaveChangesAsync();
// }

app.Run();

// ---------------------------------------------------------------------
// 11. Record-type voor demo-endpoint
// ---------------------------------------------------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
