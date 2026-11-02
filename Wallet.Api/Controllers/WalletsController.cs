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

        public WalletsController(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> WalletDetail(int id)
        {
            var wallet = await _dbContext.Wallets
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
    }
}
