using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Domain.Rules;
using Moq;
using Xunit;

namespace FraudGuard.Tests;

public class VelocityCheckRuleTests
{
    [Fact]
    public async Task EvaluateAsync_UserHasMoreThan3TransactionsIn2Minutes_ReturnsFalse()
    {
        // 1. ARRANGE (Preparar el entorno de prueba)
        var userId = Guid.NewGuid();
        var currentTransaction = new Transaction 
        { 
            UserId = userId, 
            TransactionDate = DateTime.UtcNow 
        };

        // Creamos una lista falsa de 4 transacciones previas
        var previousTransactions = new List<Transaction>
        {
            new Transaction { UserId = userId, TransactionDate = DateTime.UtcNow.AddSeconds(-10) },
            new Transaction { UserId = userId, TransactionDate = DateTime.UtcNow.AddSeconds(-30) },
            new Transaction { UserId = userId, TransactionDate = DateTime.UtcNow.AddSeconds(-60) },
            new Transaction { UserId = userId, TransactionDate = DateTime.UtcNow.AddSeconds(-90) }
        };

        // Mockeamos la base de datos (Moq)
        var mockRepo = new Mock<ITransactionRepository>();
        
        // Le decimos al mock: "Cuando alguien llame a este método, devuelve mi lista falsa"
        mockRepo
            .Setup(repo => repo.GetRecentTransactionsByUserAsync(userId, It.IsAny<DateTime>()))
            .ReturnsAsync(previousTransactions);

        // Inyectamos el mock falso en nuestra regla real
        var rule = new VelocityCheckRule(mockRepo.Object);

        // 2. ACT (Ejecutar la acción)
        var result = await rule.EvaluateAsync(currentTransaction);

        // 3. ASSERT (Verificar que el resultado es el esperado)
        Assert.False(result.IsValid);
        Assert.Equal("Rechazada por Velocity Check: Demasiadas transacciones en corto tiempo.", result.Reason);
    }
}