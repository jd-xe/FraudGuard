using FraudGuard.Application.DTOs;
using FraudGuard.Domain.Entities;
using FraudGuard.Domain.Enums;
using FraudGuard.Domain.Interfaces;
using FraudGuard.Domain.Rules;

namespace FraudGuard.Application.Services;

public class TransactionProcessor
{
    private readonly IEnumerable<IFraudRule> _rules;
    private readonly ITransactionRepository _transactionRepository;

    // INYECCIÓN MÁGICA: Al pedir un IEnumerable<IFraudRule>, .NET nos entregará
    // automáticamente una lista con todas las reglas que hayamos creado.
    public TransactionProcessor(
        IEnumerable<IFraudRule> rules, 
        ITransactionRepository transactionRepository)
    {
        _rules = rules;
        _transactionRepository = transactionRepository;
    }

    public async Task<Transaction> ProcessAsync(ProcessTransactionRequest request)
    {
        // 1. Mapear el DTO a la Entidad del Dominio
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            MerchantId = request.MerchantId,
            Amount = request.Amount,
            IpAddress = request.IpAddress,
            OriginCountry = request.OriginCountry,
            TransactionDate = DateTime.UtcNow,
            Status = TransactionStatus.Approved // Asumimos inocencia inicial
        };

        // 2. Ejecutar todas las reglas de fraude dinámicamente
        foreach (var rule in _rules)
        {
            var (isValid, reason) = await rule.EvaluateAsync(transaction);

            if (!isValid)
            {
                // Si la regla dice que es Sospechosa o Rechazada, cambiamos el estado
                transaction.Status = reason!.Contains("Rechazada") ? TransactionStatus.Rejected : TransactionStatus.Suspicious;
                
                // (Opcional por ahora: Aquí se guardaría el AuditLog)
                
                break; // Detenemos la evaluación si ya falló una regla
            }
        }

        // 3. Guardar en base de datos
        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        return transaction;
    }
}