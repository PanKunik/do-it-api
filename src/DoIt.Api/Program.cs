using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence;
using DoIt.Api.Services.Tasks;
using DoIt.Api.Controllers.Errors;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPersistance(builder.Configuration)
        .AddServices()
        .AddOwnProblemDetails()
        .AddControllers();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration[DbConstants.DoItDbConnectionStringPath]!);
}

app.Run();

