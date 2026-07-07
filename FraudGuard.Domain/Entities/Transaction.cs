using FraudGuard.Domain.Enums;

namespace FraudGuard.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MerchantId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
}