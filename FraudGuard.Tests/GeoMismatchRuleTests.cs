using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Domain.Rules;
using Moq;
using Xunit;

namespace FraudGuard.Tests;

public class GeoMismatchRuleTests
{
    [Fact]
    public async Task EvaluateAsync_CountriesDoNotMatch_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var transaction = new Transaction { UserId = userId, OriginCountry = "RU" }; // Viene de Rusia
        
        var mockUserRepo = new Mock<IUserRepository>();
        // Simulamos que la BD responde que el usuario es de Perú
        mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                    .ReturnsAsync(new User { Id = userId, CountryOfResidence = "PE" });

        var rule = new GeoMismatchRule(mockUserRepo.Object);

        // Act
        var result = await rule.EvaluateAsync(transaction);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Sospechosa: El país de origen de la transacción no coincide con la residencia del usuario.", result.Reason);
    }
}