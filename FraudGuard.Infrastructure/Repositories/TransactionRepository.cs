using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FraudGuard.Infrastructure.Repositories;

// Los dos puntos (:) significan que ESTA clase cumplirá el contrato de la interfaz
public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    // Inyectamos la base de datos real
    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetRecentTransactionsByUserAsync(Guid userId, DateTime since)
    {
        // Esto se traduce a un "SELECT * FROM Transactions WHERE UserId = ... AND Date >= ..."
        return await _context.Transactions
            .Where(t => t.UserId == userId && t.TransactionDate >= since)
            .ToListAsync();
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetSuspiciousTransactionsLast24HoursAsync(int page, int pageSize)
    {
        var yesterday = DateTime.UtcNow.AddHours(-24);

        return await _context.Transactions
            .Where(t => t.Status == FraudGuard.Domain.Enums.TransactionStatus.Suspicious && t.TransactionDate >= yesterday)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize) // Paginación: Salta los registros de páginas anteriores
            .Take(pageSize)              // Paginación: Toma solo los de la página actual
            .ToListAsync();
    }
}