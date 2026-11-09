using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Api.Data;
using Wallet.Api.Models;

using WalletModel = Wallet.Api.Models.Wallet;

namespace Wallet.Api.Test
{
    [Collection("Wallet database")]
    public class WalletDepositTests : IClassFixture<WalletTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly WalletTestWebApplicationFactory _factory;

        public WalletDepositTests(WalletTestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task WalletDeposit_ReturnNotFound()
        {
            var response = await _client.PostAsJsonAsync("/wallets/999/deposit", new { amount = "1" });
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task WalletDeposit_MissingAmount_ReturnBadRequest()
        {
            var response = await _client.PostAsJsonAsync("/wallets/999/deposit", new {});
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("-5")]
        [InlineData("1,000")]
        [InlineData("1e5")]
        [InlineData(" 50000 ")]
        [InlineData("050000")]
        [InlineData("10000.10")]
        [InlineData("1000000000000000000")]
        public async Task WalletDeposit_InvalidAmount_ReturnBadRequest(string amount)
        {
            var response = await _client.PostAsJsonAsync("/wallets/999/deposit", new { amount });
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task WalletDeposit_ReturnOk()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

            var kou = new User(firstName: "Kou", lastName: "Phounsavanh");
            var oh = new User(firstName: "Oh", lastName: "Souvannaseng");
            dbContext.Users.Add(kou);
            dbContext.Users.Add(oh);
            dbContext.SaveChanges();

            var kouWallet = new WalletModel(userId: kou.Id);
            var ohWallet = new WalletModel(userId: oh.Id);
            dbContext.Wallets.Add(kouWallet);
            dbContext.Wallets.Add(ohWallet);
            dbContext.SaveChanges();

            var depositResponse = await _client.PostAsJsonAsync($"/wallets/{kouWallet.Id}/deposit", new { amount = "50000" });
            Assert.Equal(HttpStatusCode.OK, depositResponse.StatusCode);
            var depositBody = await depositResponse.Content.ReadAsStringAsync();
            using var depositJson = JsonDocument.Parse(depositBody);
            var depositBalance = depositJson.RootElement.GetProperty("balance").GetString();
            Assert.Equal("50000", depositBalance);

            var kouWalletDetailResponse = await _client.GetAsync($"/wallets/{kouWallet.Id}");
            Assert.Equal(HttpStatusCode.OK, kouWalletDetailResponse.StatusCode);
            var kouWalletDetailBody = await kouWalletDetailResponse.Content.ReadAsStringAsync();
            using var kouWalletDetailJson = JsonDocument.Parse(kouWalletDetailBody);
            var kouWalletDetailBalance = kouWalletDetailJson.RootElement.GetProperty("balance").GetString();
            Assert.Equal("50000", kouWalletDetailBalance);

            var ohWalletDetailResponse = await _client.GetAsync($"/wallets/{ohWallet.Id}");
            Assert.Equal(HttpStatusCode.OK, ohWalletDetailResponse.StatusCode);
            var ohWalletDetailBody = await ohWalletDetailResponse.Content.ReadAsStringAsync();
            using var ohWalletDetailJson = JsonDocument.Parse(ohWalletDetailBody);
            var ohWalletDetailBalance = ohWalletDetailJson.RootElement.GetProperty("balance").GetString();
            Assert.Equal("0", ohWalletDetailBalance);
        }

    }
}
