using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        //? Methods 
        public static Error Failure(string code = "General Error", string description = "Something went wrong") => new Error(code, description, ErrorType.Failure);
        public static Error Validation(string code = "Validation Error", string description = "One or more validation errors occurred.") => new Error(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "Not Found Error", string description = "The resourse you are lookking for is not found") => new Error(code, description, ErrorType.NotFound);
        public static Error Unauthorized(string code = "Unauthorized Error", string description = "You are not authorized to access this resource") => new Error(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "Forbidden Error", string description = "You are not allowed to access this resource") => new Error(code, description, ErrorType.Forbidden);
        public static Error InvalidCredentials(string code = "InvalidCredentials Error", string description = "Invalid username or password") => new Error(code, description, ErrorType.InvalidCredentials);

    }
}