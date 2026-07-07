using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Domain.Rules;
using Moq;
using Xunit;

namespace FraudGuard.Tests;

public class BlacklistRuleTests
{
    [Fact]
    public async Task EvaluateAsync_IpIsBlocked_ReturnsFalse()
    {
        // Arrange
        var badIp = "192.168.1.100";
        var transaction = new Transaction { IpAddress = badIp, MerchantId = Guid.NewGuid() };
        
        var mockBlockRepo = new Mock<IBlockedEntityRepository>();
        // Simulamos que la base de datos dice "Sí, esta IP está en la lista negra"
        mockBlockRepo.Setup(repo => repo.IsBlockedAsync(badIp)).ReturnsAsync(true);

        var rule = new BlacklistRule(mockBlockRepo.Object);

        // Act
        var result = await rule.EvaluateAsync(transaction);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Rechazada: La entidad (IP o Comercio) se encuentra en la lista negra.", result.Reason);
    }
}