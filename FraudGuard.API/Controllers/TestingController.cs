using Bogus;
using FraudGuard.Domain.Entities;
using FraudGuard.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace FraudGuard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDatabase()
    {
        // 1. Limpiamos la base de datos
        _context.Users.RemoveRange(_context.Users);
        _context.Merchants.RemoveRange(_context.Merchants);
        _context.BlockedEntities.RemoveRange(_context.BlockedEntities);
        await _context.SaveChangesAsync();

        // 2. GOLDEN DATA: Insertar los datos exactos que espera Postman/Newman
        var qaUser = new User { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), CountryOfResidence = "PE" };
        var qaMerchant = new Merchant { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "QA Automation Co", IsActive = true };
        await _context.Users.AddAsync(qaUser);
        await _context.Merchants.AddAsync(qaMerchant);

        // 3. Bogus: Generar 50 usuarios falsos con países aleatorios
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.CountryOfResidence, f => f.Address.CountryCode());
        var fakeUsers = userFaker.Generate(50);

        // 4. Bogus: Generar 10 comercios
        var merchantFaker = new Faker<Merchant>()
            .RuleFor(m => m.Id, f => Guid.NewGuid())
            .RuleFor(m => m.Name, f => f.Company.CompanyName())
            .RuleFor(m => m.IsActive, true);
        var fakeMerchants = merchantFaker.Generate(10);

        // 4. Guardar en SQL Server
        await _context.Users.AddRangeAsync(fakeUsers);
        await _context.Merchants.AddRangeAsync(fakeMerchants);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "DB reseteada y sembrada con éxito"});
    }
}