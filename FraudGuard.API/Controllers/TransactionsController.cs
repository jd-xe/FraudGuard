using FraudGuard.Application.DTOs;
using FraudGuard.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FraudGuard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedPolicy")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionProcessor _processor;

    public TransactionsController(TransactionProcessor processor)
    {
        _processor = processor;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessTransaction([FromBody] ProcessTransactionRequest request)
    {
        //throw new Exception("Simulación de colapso de base de datos para QA.");
        // Llamamos al orquestador. Si ocurre un error, el middleware que haremos luego lo atrapará.
        var result = await _processor.ProcessAsync(request);

        return Ok(new
        {
            TransactionId = result.Id,
            Status = result.Status.ToString(),
            Message = result.Status == FraudGuard.Domain.Enums.TransactionStatus.Approved 
                ? "Transacción exitosa" 
                : "La transacción requiere revisión o fue denegada."
        });
    }

    
}