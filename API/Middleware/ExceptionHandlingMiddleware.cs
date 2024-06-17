using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware
{

    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Is invoked by the previous middleware in the pipeline.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="next">RequestDelegate to access the next middleware in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                HandleException(context, e);
            }
        }

        private void HandleException<T>(HttpContext context, T exception) where T : Exception
        {
            string message;
            int statusCode;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    message = badRequestException.Message;
                    statusCode = StatusCodes.Status400BadRequest;
                    break;

                default:
                    message = "An unexpected error occurred.";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            _logger.LogError(exception, message);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred while processing your request.",
                Detail = message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response
                .WriteAsJsonAsync(problemDetails)
                .GetAwaiter()
                .GetResult();
        }
    }
}
