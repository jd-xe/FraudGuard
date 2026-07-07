namespace FraudGuard.Domain.Interfaces;

public interface IBlockedEntityRepository
{
    // Recibe un valor (puede ser una IP o un MerchantId) y devuelve true si está bloqueado
    Task<bool> IsBlockedAsync(string value); 
}