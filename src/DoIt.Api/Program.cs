using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence;
using DoIt.Api.Services.Tasks;
using DoIt.Api.Controllers._Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<ProblemDetailsFactory, ApiProblemDetailsFactory>(); // TODO: Extract to extensions method (1)

    builder.Services
        .AddPersistance(builder.Configuration)
        .AddServices()
        .AddProblemDetails() // TODO: Extract to extensions method (2)
        .AddControllers();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration[DbConstants.DoItDbConnectionStringPath]!);
}

app.Run();

