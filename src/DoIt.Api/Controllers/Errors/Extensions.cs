using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DoIt.Api.Controllers.Errors;

public static class Extensions
{
    public static IServiceCollection AddOwnProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, ApiProblemDetailsFactory>();
        services.AddProblemDetails();
        return services;
    }
}