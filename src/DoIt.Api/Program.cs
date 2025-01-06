using DoIt.Api.Persistance.Database;
using DoIt.Api.Persistence;
using DoIt.Api.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPersistance(builder.Configuration)
        .AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration[DbConstants.DoItDbConnectionStringPath]!);
}

app.Run();

