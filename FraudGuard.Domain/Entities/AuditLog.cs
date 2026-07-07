namespace FraudGuard.Domain.Entities;
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public string FailedRuleName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; }
}