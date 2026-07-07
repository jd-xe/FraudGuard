using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;

namespace FraudGuard.Domain.Rules;

public class VelocityCheckRule : IFraudRule
{
    private readonly ITransactionRepository _repository;
    private const int MaxTransactions = 3;
    private const int TimeWindowMinutes = 2;

    public string RuleName => "VelocityCheck";

    // Inyección de dependencias: La regla recibe el repositorio, pero no sabe qué base de datos es.
    public VelocityCheckRule(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool IsValid, string? Reason)> EvaluateAsync(Transaction transaction)
    {
        // Calculamos el límite de tiempo (hace 2 minutos)
        var timeLimit = transaction.TransactionDate.AddMinutes(-TimeWindowMinutes);

        // Consultamos el historial del usuario (en el test, esto será respondido por el Mock)
        var recentTransactions = await _repository.GetRecentTransactionsByUserAsync(transaction.UserId, timeLimit);

        // Lógica de negocio: Si ya tiene 3 o más transacciones, rechazamos.
        if (recentTransactions.Count >= MaxTransactions)
        {
            return (false, "Rechazada por Velocity Check: Demasiadas transacciones en corto tiempo.");
        }

        return (true, null); // La transacción es válida para esta regla
    }
}