using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence;
using DoIt.Api.Controllers.Errors;
using DoIt.Api.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPersistence(builder.Configuration)
        .AddServices()
        .AddOwnProblemDetails()
        .AddControllers();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error"); // TODO: Check
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration[DbConstants.DoItDbConnectionStringPath]!);
}

app.Run();

