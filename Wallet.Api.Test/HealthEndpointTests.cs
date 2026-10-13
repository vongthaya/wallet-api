using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Wallet.Api.Test;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnOk()
    {
       var responseMessage = await _client.GetAsync("/health");
       var body = await responseMessage.Content.ReadAsStringAsync();
       using var json = JsonDocument.Parse(body);
       var statusText = json.RootElement.GetProperty("status").GetString();
       Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
       Assert.Equal("ok", statusText);
    }
}
