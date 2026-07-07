using FraudGuard.Domain.Entities;

namespace FraudGuard.Domain.Interfaces;

public interface ITransactionRepository
{
    // Obtiene las transacciones de un usuario en un rango de tiempo
    Task<List<Transaction>> GetRecentTransactionsByUserAsync(Guid userId, DateTime since);
    Task AddAsync(Transaction transaction);
    Task SaveChangesAsync();
    Task<List<Transaction>> GetSuspiciousTransactionsLast24HoursAsync(int page, int pageSize);
}