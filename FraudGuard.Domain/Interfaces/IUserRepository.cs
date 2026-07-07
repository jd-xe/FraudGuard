using FraudGuard.Domain.Entities;
namespace FraudGuard.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
}