using Microsoft.AspNetCore.Diagnostics;
using TryMongoDB.Application.Common;

namespace TryMongoDB.ExceptionHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var result = exception switch
        {
            ArgumentException argumentException => ApplicationResult<object>.Failure(
                ApplicationErrorType.Validation,
                argumentException.Message),

            _ => ApplicationResult<object>.Failure(
                ApplicationErrorType.Unexpected,
                "An unexpected error occurred.")
        };

        if (result.ErrorType == ApplicationErrorType.Unexpected)
        {
            logger.LogError(exception, "Unhandled exception occurred.");
        }

        httpContext.Response.StatusCode = result.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}
