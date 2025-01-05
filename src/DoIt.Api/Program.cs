using DoIt.Api.Persistance.Database;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration["Database:ConnectionStrings:DoItDb"]!);
}

app.Run();

