using FraudGuard.Domain.Entities;

namespace FraudGuard.Domain.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(AuditLog auditLog);
}