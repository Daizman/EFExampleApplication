using System.Net;
using System.Net.Mime;
using EFExampleApplication.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace EFExampleApplication.Services;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case UserNotFoundException
                or MovieNotFoundException
                or GenreNotFoundException
                or ReviewNotFoundException:
                httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsync(exception.Message, cancellationToken: cancellationToken);
                return true;
        }

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsync(string.Empty, cancellationToken: cancellationToken);

        return false;
    }
}
