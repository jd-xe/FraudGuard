namespace FraudGuard.Domain.Entities;
public class Merchant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}