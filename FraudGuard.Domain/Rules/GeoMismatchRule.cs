using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;

namespace FraudGuard.Domain.Rules;

public class GeoMismatchRule : IFraudRule
{
    private readonly IUserRepository _userRepository;
    public string RuleName => "GeoMismatch";

    public GeoMismatchRule(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool IsValid, string? Reason)> EvaluateAsync(Transaction transaction)
    {
        var user = await _userRepository.GetByIdAsync(transaction.UserId);

        // Si el usuario no existe (caso borde) o los países son diferentes
        if (user == null || !transaction.OriginCountry.Equals(user.CountryOfResidence, StringComparison.OrdinalIgnoreCase))
        {
            return (false, "Sospechosa: El país de origen de la transacción no coincide con la residencia del usuario.");
        }

        return (true, null);
    }
}