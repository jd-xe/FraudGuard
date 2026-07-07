using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Infrastructure.Data;

namespace FraudGuard.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        // Busca al usuario por su ID en la base de datos real
        return await _context.Users.FindAsync(userId);
    }
}