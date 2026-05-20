using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        // in this class we will add all our api controllers
        // we will add my code to validate the request and response 
        // and use it in all our api controllers

        //? CommonResult
        //? Handle Result

        // Handle Request without Values:-
        // ? If Result success => Returen NoContent [204]
        // ? If Result failure => return Problem Details with status code Error Details
        protected ActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return HandleProblem(result.GetErrors());
        }

        // Handle Request with Values:-
        // ? If Result success => Returen Ok [200]
        // ? If Result failure => return Problem Details with status code Error Details
        protected  ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return HandleProblem(result.GetErrors());
        }


        //////////////////////////////////

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            //? If no Errors occured => Returen Problem Details with status code 500
            //? If multiple Errors occured => return Problem Details with status code Error Details as Validation Problem
            //? If one Error occured => return Problem Details with status code Error Details

            if(errors.Count == 0) return Problem(statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: "Something went wrong");

            if(errors.All(E => E.Type == ErrorType.Validation)) return HandleValidationProblem(errors);

            return HandleSingleErrorProblem(errors.First());

           
        }

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            //ModelState => this is the Validation Errors
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type)
            );
        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }

    }
}