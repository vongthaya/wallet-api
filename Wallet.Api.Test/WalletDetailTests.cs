using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Api.Data;
using Wallet.Api.Models;
using WalletModel = Wallet.Api.Models.Wallet;

namespace Wallet.Api.Test;

public class WalletDetailTests : IClassFixture<WalletTestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly WalletTestWebApplicationFactory _factory;

    public WalletDetailTests(WalletTestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
        dbContext.Wallets.ExecuteDelete();
        dbContext.Users.ExecuteDelete();
    }

    [Fact]
    public async Task GetWalletDetail_ReturnNotFound()
    {
        var response = await _client.GetAsync("/wallets/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetWalletDetail_ReturnOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

        var tempUser = new User(
            firstName: "Temp",
            lastName: "Temp"
        );
        var user = new User(
            firstName: "Kou",
            lastName: "Phounsavanh"
        );
        dbContext.Users.Add(tempUser);
        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        var wallet = new WalletModel(
            userId: user.Id
        );
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        Assert.NotEqual(user.Id, wallet.Id);

        var response = await _client.GetAsync($"/wallets/{wallet.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        var walletId = json.RootElement.GetProperty("id").GetInt32();
        var balance = json.RootElement.GetProperty("balance").GetString();
        var currency = json.RootElement.GetProperty("currency").GetString();
        var owner = json.RootElement.GetProperty("owner");
        var ownerId = owner.GetProperty("id").GetInt32();
        var ownerFirstName = owner.GetProperty("firstName").GetString();
        var ownerLastName = owner.GetProperty("lastName").GetString();

        Assert.Equal(wallet.Id, walletId);
        Assert.Equal("0", balance);
        Assert.Equal(user.Id, ownerId);
        Assert.Equal("Kou", ownerFirstName);
        Assert.Equal("Phounsavanh", ownerLastName);
        Assert.Equal("LAK", currency);
    }

    [Fact]
    public async Task GetWalletDetail_ReturnMethodNotAllow()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

        var user = new User(
            firstName: "Kou",
            lastName: "Phounsavanh"
        );
        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        var wallet = new WalletModel(
            userId: user.Id
        );
        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        var response = await _client.PostAsync($"/wallets/{wallet.Id}", null);

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }
}
