using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Infrastructure.Data;

namespace FraudGuard.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly ApplicationDbContext _context;

    public AuditRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
    }
}