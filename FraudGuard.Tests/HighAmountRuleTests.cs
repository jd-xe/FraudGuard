using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Rules;
using Xunit;

namespace FraudGuard.Tests;

public class HighAmountRuleTests
{
    [Fact]
    public async Task EvaluateAsync_AmountIsGreaterThan5000_ReturnsFalse()
    {
        // Arrange
        var transaction = new Transaction { Amount = 5001m }; // La 'm' indica que es decimal
        var rule = new HighAmountRule();

        // Act
        var result = await rule.EvaluateAsync(transaction);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Sospechosa: El monto supera los $5,000 USD.", result.Reason);
    }
}