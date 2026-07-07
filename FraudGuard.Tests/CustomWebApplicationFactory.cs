using FraudGuard.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FraudGuard.Tests;

// Heredamos de WebApplicationFactory apuntando al punto de entrada (Program) de nuestra API
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // 1. Buscar y remover la configuración real de SQL Server que pusimos en Program.cs
            /*var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }*/

            // 1. Borramos las opciones clásicas (Lo que ya hacíamos)
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            services.RemoveAll(typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));

            // 2. Reemplazarla con una base de datos en memoria exclusiva para este test
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
            });
        });
    }
}