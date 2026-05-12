using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.Presentation.Attributes
{
    // This attribute is used to cache the response of an action method in Redis for a specified duration.
    // use it on any request you want to cache its response for example get all products
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMinutes;

        public RedisCacheAttribute(int durationInMinutes = 5)
        {
            _durationInMinutes = durationInMinutes;
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    // Implement caching logic here, e.g., check Redis cache for existing data
        //    // If data exists in cache, return it and skip the action execution
        //    // If not, proceed with the action execution and store the result in cache
        //}

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    // Implement logic to store the result of the action execution in Redis cache
        //}

        // Executed after the action method is invoked, but before the result is executed.

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // context : this is the request (controle on request)
            // next : this is like pointer to implement after caching 

            // Get Cache Service
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>(); // Registered in programs

            // Check if cached datat exist?
            // Create cacheKey
            var cacheKey = CreateCacheKey(context.HttpContext.Request);


            // If Exist => Returen Cached Data and skep executing the endpoint 
            var cacheValue = await CacheService.GetAsync(cacheKey);
            if (cacheValue != null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // If Not Exist 
            var ExecutedContext = await next.Invoke(); // Execute the endpoint
            if (ExecutedContext.Result is OkObjectResult objectResult)
            {
                // Store the result in cache
                await CacheService.SetAsync(cacheKey, objectResult.Value!, TimeSpan.FromMinutes(_durationInMinutes));
            }




        }

        private string CreateCacheKey(HttpRequest request)
        {
            // Use url to make it as key in cache but there are parametes to filter 
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                // api/products/brandId=1&typeId=2
                // api/products/typeId=2&brandId=1
                key.Append($"|{item.Key}={item.Value}");
            }
            return key.ToString();
        }

    }
}
