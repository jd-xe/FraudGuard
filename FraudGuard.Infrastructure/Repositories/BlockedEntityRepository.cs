using FraudGuard.Domain.Interfaces;
using FraudGuard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FraudGuard.Infrastructure.Repositories;

public class BlockedEntityRepository : IBlockedEntityRepository
{
    private readonly ApplicationDbContext _context;

    public BlockedEntityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsBlockedAsync(string value)
    {
        // Devuelve 'true' si encuentra la IP o el MerchantId en la lista negra
        return await _context.BlockedEntities.AnyAsync(b => b.Value == value);
    }
}