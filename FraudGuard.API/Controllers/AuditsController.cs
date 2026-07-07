using FraudGuard.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FraudGuard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditsController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;

    public AuditsController(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    [HttpGet("suspicious")]
    public async Task<IActionResult> GetSuspiciousTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // Validación básica
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Parámetros de paginación inválidos. La página debe ser > 0 y el tamaño <= 100.");
        }

        var suspiciousTransactions = await _transactionRepository.GetSuspiciousTransactionsLast24HoursAsync(page, pageSize);

        // Retornamos el listado y metadatos de paginación
        return Ok(new
        {
            Page = page,
            PageSize = pageSize,
            Count = suspiciousTransactions.Count,
            Data = suspiciousTransactions
        });
    }
}