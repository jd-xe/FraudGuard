namespace FraudGuard.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string CountryOfResidence { get; set; } = string.Empty;
}