using DoIt.Api.Services.TaskLists;
using DoIt.Api.Services.Tasks;

namespace DoIt.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITasksService, TasksService>();
        services.AddScoped<ITaskListsService, TaskListsService>();
        return services;
    }
}