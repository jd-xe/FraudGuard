using System.Net.Http.Json;
using FraudGuard.Application.DTOs;
using FraudGuard.Domain.Entities;
using FraudGuard.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FraudGuard.Tests;

public class TransactionsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    // El constructor recibe nuestra factoría gracias a IClassFixture
    public TransactionsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(); // Creamos el cliente HTTP en memoria
    }

    private record ProcessResponse(Guid TransactionId, string Status, string Message);

    [Fact]
    public async Task ProcessTransaction_ValidPayload_Returns200OkAndSavesToDb()
    {
        // 1. ARRANGE: Preparar datos requeridos en la BD en memoria
        var userId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Creamos un usuario que reside en Perú (PE) para que la regla de GeoMismatch no falle
            await db.Users.AddAsync(new User { Id = userId, CountryOfResidence = "PE" });
            await db.SaveChangesAsync();
        }

        // Creamos el JSON que simula la petición externa del cliente
        var requestPayload = new ProcessTransactionRequest(
            UserId: userId,
            MerchantId: merchantId,
            Amount: 100m,            // Monto bajo (pasa HighAmountRule)
            IpAddress: "182.16.0.1", // IP limpia (pasa BlacklistRule)
            OriginCountry: "PE"      // Mismo país del usuario (pasa GeoMismatchRule)
        );

        // 2. ACT: Disparar la petición HTTP POST real hacia la API en memoria
        var response = await _client.PostAsJsonAsync("/api/transactions/process", requestPayload);

        // 3. ASSERT: Validar las capas de transporte y persistencia
        // Validación A: La API respondió con éxito en la capa HTTP
        response.EnsureSuccessStatusCode(); // Verifica código 200-299
        
        var jsonResult = await response.Content.ReadFromJsonAsync<ProcessResponse>();
        Assert.NotNull(jsonResult);

        // Validación B: Verificar que los datos realmente atravesaron el orquestador y se guardaron en las tablas
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var savedTransaction = await db.Transactions.FindAsync(jsonResult!.TransactionId);
            
            Assert.NotNull(savedTransaction);
            Assert.Equal(FraudGuard.Domain.Enums.TransactionStatus.Approved, savedTransaction.Status);
        }
    }
}