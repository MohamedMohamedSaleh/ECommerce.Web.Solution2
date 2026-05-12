using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {

        // You must inject RequestDelegate as Next Middleware to your CTOR
        // Must have invodkeAsync method with HttpContext as parameter

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                // 404 NotFound 
                if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
                {
                    var Problem = new ProblemDetails()
                    {
                        Title = "Response Not Found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = "The resourse you are lookking for is not found",
                        Instance = context.Request.Path
                    };
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(Problem);
                }
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails()
                {
                    Title = "Response Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails()
                {
                    Title = "An unexpected error occurred!",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem);
            }

        }
    }
}
