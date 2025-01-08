using System.Net;

namespace DoIt.Api.Integration.Tests.App;

public class AppControllerTests
    : IClassFixture<DoItApiFactory>
{
    private readonly HttpClient _client;

    public AppControllerTests(DoItApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldReturnOk()
    {
        // Act
        var result = await _client.GetAsync("api/app");

        // Assert
        result
            .Should()
            .NotBeNull();

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        (await result.Content
            .ReadAsStringAsync())
            .Should()
            .Be("200");
    }
}
