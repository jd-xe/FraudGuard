namespace FraudGuard.Application.DTOs;

public record ProcessTransactionRequest(
    Guid UserId, 
    Guid MerchantId, 
    decimal Amount, 
    string IpAddress, 
    string OriginCountry
);