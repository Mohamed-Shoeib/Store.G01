using Domain.Exceptions;
using Shared.ErrorsModel;

namespace Store.G02.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate _next, ILogger<GlobalErrorHandlingMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch(Exception ex)
            {
                // Log The Exception
                logger.LogError(ex, ex.Message);
                await HandlingError(context, ex);
            }
        }

        private static async Task HandlingError(HttpContext context, Exception ex)
        {
            // 1. Set Status Code For Response
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // 2. Set Content Type For Response
            context.Response.ContentType = "application/json";

            // 3. Response Object (Body)
            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };
            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                VaildationException => HandlingVaildationExceptionAsync(response, (VaildationException)ex),
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            // 4. Return Response
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"EndPoint {context.Request.Path} Is Not Found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        private static int HandlingVaildationExceptionAsync(ErrorDetails response, VaildationException ex)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
