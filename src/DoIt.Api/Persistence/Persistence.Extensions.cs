using DoIt.Api.Persistence.Database;
using DoIt.Api.Persistence.Repositories.TaskLists;
using DoIt.Api.Persistence.Repositories.Tasks;

namespace DoIt.Api.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(configuration[DbConstants.DoItDbConnectionStringPath]!));
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<ITaskListsRepository, TaskListsRepository>();

        return services;
    }
}
