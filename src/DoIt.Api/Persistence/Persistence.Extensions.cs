using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.AssignmentsLists;
using DoIt.Api.Persistence.Repositories.Assignments;

namespace DoIt.Api.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(configuration[DbConstants.DoItDbConnectionStringPath]!));
        services.AddScoped<IAssignmentsRepository, AssignmentsRepository>();
        services.AddScoped<IAssignmentsListsRepository, AssignmentsListsRepository>();
        return services;
    }
}