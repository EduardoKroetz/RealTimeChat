using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.DTOs;
using System.Net;

namespace RealTimeChat.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var result = Result.FailureResult(exception.Message);

        switch (exception)
        {
            case ArgumentException e:
            case InvalidOperationException:
                httpContext.Response.StatusCode = 400;
            break;
            case UnauthorizedAccessException e:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            break;
            case DbUpdateException _:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = Result.FailureResult("Unable to update database");
            break;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}
