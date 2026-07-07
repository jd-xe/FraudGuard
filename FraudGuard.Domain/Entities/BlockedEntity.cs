namespace FraudGuard.Domain.Entities;
public class BlockedEntity
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty; // Aquí guardaremos la IP o el MerchantId
    public DateTime BlockedAt { get; set; }
}