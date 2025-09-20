using MiniBank.Api.Models;

namespace MiniBank.Api.Services
{
    public record OpenAccountDto(string Currency);
    public record MoneyDto(decimal Amount);
    public record TransferDto(int FromAccountId, int ToAccountId, decimal Amount, string? Description);

    public interface IAccountService
    {
        Task<Account> OpenAsync(int customerId, OpenAccountDto dto);
        Task DepositAsync(int customerId, int accountId, MoneyDto dto);
        Task WithdrawAsync(int customerId, int accountId, MoneyDto dto);
        Task TransferAsync(int customerId, TransferDto dto);
        Task<IReadOnlyList<Transaction>> StatementAsync(int customerId, int accountId, DateTime? from, DateTime? to);
    }
}
