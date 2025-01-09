using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence;
using DoIt.Api.Services.Tasks;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPersistance(builder.Configuration)
        .AddServices()
        .AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration[DbConstants.DoItDbConnectionStringPath]!);
}

app.Run();

