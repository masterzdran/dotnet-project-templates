using System.Collections.Generic;
using Extended.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Extended.WebApi.Helpers
{
    
internal static class ValidationResultHelper
    {
        public static IActionResult CreateValidationErrorBadRequestResponse(string message, Dictionary<string, string[]> errors)
        {
            var problemDetails = new ValidationProblemDetails(errors)
            {
                Detail = message,
                Status = StatusCodes.Status400BadRequest,
                Type = IValidationResultType.ValidationException,
            };
            
            return new BadRequestObjectResult(problemDetails);
        }
        
        public static IActionResult CreateBadRequestResponse(string message, ModelStateDictionary modelState)
        {
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Detail = message,
                Status = StatusCodes.Status400BadRequest,
                Type = IValidationResultType.ValidationException,
            };
            return new BadRequestObjectResult(problemDetails);
        }
        
        public static IActionResult CreateInternalServerErrorResponse(string message)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = message,
                Status = StatusCodes.Status500InternalServerError,
                Type = IValidationResultType.GeneralException,
            };
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
            };
        }
        
        public static IActionResult CreateConflictResponse(string message)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Detail = message,
                Status = StatusCodes.Status409Conflict,
                Type = IValidationResultType.ConflictException,
            };
            return new ConflictObjectResult(problemDetails);
        }
        
        public static IActionResult CreateNotFoundResponse(string message)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = message,
                Status = StatusCodes.Status404NotFound,
                Type = IValidationResultType.NotFoundException,
            };
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
            };
        }
    }
}