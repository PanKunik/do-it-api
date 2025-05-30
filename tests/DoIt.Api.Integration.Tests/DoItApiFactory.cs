using System.Data.Common;
using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.Assignments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    public HttpClient HttpClient { get; private set; } = null!;

    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

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

        builder.ConfigureServices(
            services =>
            {
                services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(_postgresContainer.GetConnectionString()));
                services.AddSingleton<IAssignmentsRepository, AssignmentsRepository>();
                services.AddSingleton<IAssignmentsListsRepository, AssignmentsListsRepository>();
            });
    }

    public new async Task DisposeAsync() => await _postgresContainer.StopAsync();
}