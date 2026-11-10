using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Data;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("wallets")]
    public class WalletsController : ControllerBase
    {
        private readonly WalletDbContext _dbContext;
        public record DepositRequest(string Amount);

        public WalletsController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> WalletDetail(int id)
        {
            var wallet = await GetWalletDetail(id);
            if (wallet == null)
            {
                return Problem(
                    statusCode: 404,
                    title: "Not Found",
                    detail: $"Wallet {id} was not found"
                );
            }
            return Ok(wallet);
        }

        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> WalletDeposit(int id, DepositRequest request)
        {
            if (!Regex.IsMatch(request.Amount, @"^[1-9][0-9]{0,17}\z"))
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Bad Request", detail: "Invalid amount");
            }
            var wallet = await _dbContext.Wallets
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();
            if (wallet == null)
            {
                return Problem(statusCode: 404, title: "Not Found", detail: $"Wallet {id} was not found");
            }

            var amount = decimal.Parse(request.Amount);

            try
            {
                wallet.Deposit(amount);
            }
            catch (InvalidOperationException)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, title: "Conflict", detail: "Deposit would exceed the maximum balance");
            }

            await _dbContext.SaveChangesAsync();

            var walletResponse = await GetWalletDetail(id);

            return Ok(walletResponse);
        }

        private async Task<object?> GetWalletDetail(int id)
        {
            return await _dbContext.Wallets
                .Where(w => w.Id == id)
                .Select(w => new
                {
                    id = w.Id,
                    owner = new
                    {
                        id = w.Owner.Id,
                        firstName = w.Owner.FirstName,
                        lastName = w.Owner.LastName,
                    },
                    balance = w.Balance.ToString(),
                    currency = "LAK",
                })
                .FirstOrDefaultAsync();
        }
    }
}
