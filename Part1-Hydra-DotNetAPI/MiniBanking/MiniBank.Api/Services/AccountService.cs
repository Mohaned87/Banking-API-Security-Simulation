using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;
using MiniBank.Api.Models;

namespace MiniBank.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDb _db;
        public AccountService(AppDb db) => _db = db;

        private Task<Account> GetOwnedAccount(int customerId, int accountId) =>
        _db.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.CustomerId == customerId)
        .ContinueWith(t => t.Result ?? throw new Exception("Account not found or not yours"));

        public async Task<Account> OpenAsync(int customerId, OpenAccountDto dto)
        {
            var acc = new Account
            {
                CustomerId = customerId,
                Currency = dto.Currency,
                AccountNumber = $"AC{DateTime.UtcNow.Ticks}"
            };
            _db.Accounts.Add(acc);
            await _db.SaveChangesAsync();
            return acc;
        }

        public async Task DepositAsync(int customerId, int accountId, MoneyDto dto)
        {
            if (dto.Amount <= 0) throw new Exception("Invalid amount");
            var acc = await GetOwnedAccount(customerId, accountId);

            using var tx = await _db.Database.BeginTransactionAsync();
            acc.Balance += dto.Amount;
            _db.Transactions.Add(new Transaction
            {
                Type = TxType.Deposit,
                Amount = dto.Amount,
                ToAccountId = acc.Id,
                Description = "Deposit"
            });
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task WithdrawAsync(int customerId, int accountId, MoneyDto dto)
        {
            if (dto.Amount <= 0) throw new Exception("Invalid amount");
            var acc = await GetOwnedAccount(customerId, accountId);
            if (acc.Balance < dto.Amount) throw new Exception("Insufficient funds");

            using var tx = await _db.Database.BeginTransactionAsync();
            acc.Balance -= dto.Amount;
            _db.Transactions.Add(new Transaction
            {
                Type = TxType.Withdraw,
                Amount = dto.Amount,
                FromAccountId = acc.Id,
                Description = "Withdraw"
            });
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task TransferAsync(int customerId, TransferDto dto)
        {
            if (dto.Amount <= 0) throw new Exception("Invalid amount");
            var from = await GetOwnedAccount(customerId, dto.FromAccountId);
            var to = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == dto.ToAccountId)
                     ?? throw new Exception("Target account not found");
            if (from.Currency != to.Currency) throw new Exception("Different currency");
            if (from.Balance < dto.Amount) throw new Exception("Insufficient funds");

            using var tx = await _db.Database.BeginTransactionAsync();
            from.Balance -= dto.Amount;
            to.Balance += dto.Amount;
            _db.Transactions.Add(new Transaction
            {
                Type = TxType.Transfer,
                Amount = dto.Amount,
                FromAccountId = from.Id,
                ToAccountId = to.Id,
                Description = dto.Description ?? "Transfer"
            });
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task<IReadOnlyList<Transaction>> StatementAsync(int customerId, int accountId, DateTime? from, DateTime? to)
        {
            await GetOwnedAccount(customerId, accountId);
            var q = _db.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId);
            if (from.HasValue) q = q.Where(t => t.UtcAt >= from.Value);
            if (to.HasValue) q = q.Where(t => t.UtcAt <= to.Value);
            return await q.OrderByDescending(t => t.UtcAt).ToListAsync();
        }
    }
}
