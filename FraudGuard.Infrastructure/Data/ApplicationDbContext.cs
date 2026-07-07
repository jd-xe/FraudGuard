using FraudGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FraudGuard.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Estas propiedades representan las 5 tablas en SQL Server
    public DbSet<User> Users { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BlockedEntity> BlockedEntities { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}