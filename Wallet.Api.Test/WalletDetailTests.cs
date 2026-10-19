using System.Net;

namespace Wallet.Api.Test;

public class WalletDetailTests : IClassFixture<WalletTestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WalletDetailTests(WalletTestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetWalletDetail_ReturnNotFound()
    {
        var response = await _client.GetAsync("/wallets/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
