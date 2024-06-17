using Core.Exceptions;

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
            string message = "An unexpected error occurred.";
            int statusCode = StatusCodes.Status500InternalServerError;

            _logger.LogError(exception, message);

            context.Response.StatusCode = statusCode;
            context.Response
                .WriteAsJsonAsync(message)
                .GetAwaiter()
                .GetResult();
        }
    }
}
