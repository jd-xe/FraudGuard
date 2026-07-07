using FraudGuard.Domain.Entities;

namespace FraudGuard.Domain.Rules;

public interface IFraudRule
{
    string RuleName { get; }
    Task<(bool IsValid, string? Reason)> EvaluateAsync(Transaction transaction);
}