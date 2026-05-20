using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Result
    {
        //? IsSuccess
        //? IsFailure
        //? Errors [Code - Description - Type] => Generate Class Error

        protected readonly List<Error> _errors = new List<Error>();

        public bool IsSuccess => !_errors.Any();

        public bool IsFailure => !IsSuccess;

        // property to get errors
        public IReadOnlyList<Error> GetErrors() => _errors;




        // No Errors  => 
        protected Result (){}
        // One Error occured => 
        protected Result (Error error) => _errors.Add(error);
        // Multiple Errors occured => 
        protected Result (List<Error> errors) => _errors.AddRange(errors);


        // Methods to Call Constructor

        // if no errors occured
        public static Result Ok() => new Result();

        // if one error occured
        public static Result Failure(Error error) => new Result(error);
        
        // if multiple errors occured
        public static Result Failure(List<Error> errors) => new Result(errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot create a success result with a value");

        private Result(TValue value) : base()
        {
            _value = value;
        }

        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Failure(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Failure(List<Error> errors) => new Result<TValue>(errors);

        // Implicit Casting
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Failure(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);
    }

}