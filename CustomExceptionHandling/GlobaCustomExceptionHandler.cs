using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace ConfidentVotesAPI.CustomExceptionHandling
{
    

public sealed class GlobalExceptionHandler
    : IExceptionHandler
{   
    // logger
    private readonly ILogger<GlobalExceptionHandler> _logger;
    // logger initialization in the constructor
    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    // 
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ConfidentAPIException apiException)
        {
            context.Response.StatusCode =
                apiException.StatusCode;

            await context.Response.WriteAsJsonAsync(
                // best for giving clear readable eror response
                new ProblemDetails
                {
                    Status = apiException.StatusCode,
                    Title = apiException.ErrorCode,
                    Detail = apiException.Message
                },
                cancellationToken);

            return true;
        }

        _logger.LogError( exception, "Unexpected exception.");

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = 500,
                Title = "INTERNAL_SERVER_ERROR",
                Detail = "An unexpected error occurred."
            },
            cancellationToken);

        return true;
    }
}
}