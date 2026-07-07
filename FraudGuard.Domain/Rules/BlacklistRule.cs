using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;

namespace FraudGuard.Domain.Rules;

public class BlacklistRule : IFraudRule
{
    private readonly IBlockedEntityRepository _blockedRepo;
    public string RuleName => "Blacklist";

    public BlacklistRule(IBlockedEntityRepository blockedRepo)
    {
        _blockedRepo = blockedRepo;
    }

    public async Task<(bool IsValid, string? Reason)> EvaluateAsync(Transaction transaction)
    {
        // Verificamos tanto la IP como el ID del comercio
        bool isIpBlocked = await _blockedRepo.IsBlockedAsync(transaction.IpAddress);
        bool isMerchantBlocked = await _blockedRepo.IsBlockedAsync(transaction.MerchantId.ToString());

        if (isIpBlocked || isMerchantBlocked)
        {
            return (false, "Rechazada: La entidad (IP o Comercio) se encuentra en la lista negra.");
        }

        return (true, null);
    }
}