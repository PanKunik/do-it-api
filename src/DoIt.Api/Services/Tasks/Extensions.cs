namespace DoIt.Api.Services.Tasks;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services
    )
    {
        services.AddScoped<ITasksService, TasksService>();

        return services;
    }
}
