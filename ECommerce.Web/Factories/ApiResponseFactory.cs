using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext context)
        {
            var errors = context.ModelState
                       .Where(x => x.Value?.Errors.Count > 0)
                       .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value?.Errors
                               .Select(e => e.ErrorMessage)
                               .ToArray()
                       );

            var validationProblemDetails = new ValidationProblemDetails(errors!)
            {
                Title = "Validation Error!",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Instance = context.HttpContext.Request.Path
            };

            return new BadRequestObjectResult(validationProblemDetails);
        }

    }
}
