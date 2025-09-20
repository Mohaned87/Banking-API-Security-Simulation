using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Services;
using Microsoft.EntityFrameworkCore;
using MiniBank.Api.Data;


namespace MiniBank.Api.Controllers
{
    [ApiController, Route("api/accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _svc;
        private readonly AppDb _db;
        //public AccountsController(IAccountService svc) { _svc = svc; }
        public AccountsController(IAccountService svc, AppDb db) { _svc = svc; _db = db; }

        private int CustomerId => int.Parse(User.FindFirst("customerId")!.Value);

        [HttpPost] // ⬅️ /api/accounts
        public async Task<IActionResult> Open(OpenAccountDto dto)
            => Ok(await _svc.OpenAsync(CustomerId, dto));

        [HttpPost("{id:int}/deposit")]
        public async Task<IActionResult> Deposit(int id, MoneyDto dto)
        { await _svc.DepositAsync(CustomerId, id, dto); return Ok(); }

        [HttpPost("{id:int}/withdraw")]
        public async Task<IActionResult> Withdraw(int id, MoneyDto dto)
        { await _svc.WithdrawAsync(CustomerId, id, dto); return Ok(); }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto)
        { await _svc.TransferAsync(CustomerId, dto); return Ok(); }

        [HttpGet("{id:int}/statement")]
        public async Task<IActionResult> Statement(int id, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
            => Ok(await _svc.StatementAsync(CustomerId, id, from, to));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var customerId = int.Parse(User.FindFirst("customerId")!.Value);
            var acc = await _db.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == customerId);

            if (acc is null) return NotFound();
            return Ok(acc);
        }

    }

}
