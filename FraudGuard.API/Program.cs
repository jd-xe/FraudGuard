using FraudGuard.Infrastructure.Data;
using FraudGuard.Infrastructure.Repositories;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Domain.Rules;
using FraudGuard.Application.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using FraudGuard.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Conectar EF Core a SQL Server usando la cadena del appsettings
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Rate Limiting: Máximo 5 peticiones por segundo
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(1);
        opt.PermitLimit = 5;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// 3. Inyección de Repositorios
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlockedEntityRepository, BlockedEntityRepository>();

// 4. Inyección del Orquestador
builder.Services.AddScoped<TransactionProcessor>();

// 5. Inyección de Reglas de Negocio
builder.Services.AddScoped<IFraudRule, HighAmountRule>();
builder.Services.AddScoped<IFraudRule, VelocityCheckRule>();
builder.Services.AddScoped<IFraudRule, GeoMismatchRule>();
builder.Services.AddScoped<IFraudRule, BlacklistRule>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// 6. Activar nuestro Middleware Global de Errores (Debe ir primero)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. Activar el Rate Limiter antes de mapear los controladores
app.UseRateLimiter();

app.MapControllers();
app.Run();

public partial class Program { }