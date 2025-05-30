using DoIt.Api.Services.AssignmentsLists;
using DoIt.Api.Services.Assignments;

namespace DoIt.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAssignmentsService, AssignmentsService>();
        services.AddScoped<IAssignmentsListsService, AssignmentsListsService>();
        return services;
    }
}