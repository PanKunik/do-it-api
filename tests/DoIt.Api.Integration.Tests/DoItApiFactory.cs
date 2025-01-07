using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace DoIt.Api.Integration.Tests;

public class DoItApiFactory
    : WebApplicationFactory<IApiMarker>
    , IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("do-it")
        .WithUsername("root")
        .WithPassword("123456")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseConfiguration(
                new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            { "Database:ConnectionStrings:DoItDb", _postgresContainer.GetConnectionString() }
                        }!
                    )
                    .Build()
            );
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
    }
}
