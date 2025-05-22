using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
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

    public HttpClient HttpClient { get; private set; } = default!;

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_postgresContainer.GetConnectionString());
        HttpClient = CreateClient();
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            }
        );
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