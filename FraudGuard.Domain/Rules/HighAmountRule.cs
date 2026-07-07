using FraudGuard.Domain.Entities;

namespace FraudGuard.Domain.Rules;

public class HighAmountRule : IFraudRule
{
    public string RuleName => "HighAmount";
    private const decimal MaxAmount = 5000m;

    public Task<(bool IsValid, string? Reason)> EvaluateAsync(Transaction transaction)
    {
        if (transaction.Amount > MaxAmount)
        {
            //return Task.FromResult((false, "Sospechosa: El monto supera los $5,000 USD."));
            return Task.FromResult<(bool IsValid, string? Reason)>((false, "Sospechosa: El monto supera los $5,000 USD."));
        }

        return Task.FromResult((true, (string?)null));
    }
}