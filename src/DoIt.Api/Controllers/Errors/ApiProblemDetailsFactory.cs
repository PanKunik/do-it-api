using System.Diagnostics;
using DoIt.Api.Shared.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace DoIt.Api.Controllers.Errors;

public class ApiProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options = options?.Value
        ?? throw new ArgumentNullException(nameof(options));

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(
            httpContext,
            problemDetails,
            statusCode.Value
        );

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        if (modelStateDictionary is null)
            throw new ArgumentNullException(nameof(modelStateDictionary));

        statusCode ??= 400;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        if (title is not null)
            problemDetails.Title = title;

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(
        HttpContext httpContext,
        ProblemDetails problemDetails,
        int statusCode
    )
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        problemDetails.Instance = httpContext.Request.Path;

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId is not null)    // TODO: Check if we need to add this 'traceId' manually
            problemDetails.Extensions[Constants.Error.TraceIdName] = traceId;

        if (httpContext?.Items[Constants.Error.ErrorsName] is Error error)
        {
            problemDetails.Title = error.Code;
            problemDetails.Detail = error.Message;
        }
    }
}