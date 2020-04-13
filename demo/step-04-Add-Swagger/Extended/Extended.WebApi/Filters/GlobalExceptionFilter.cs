using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net;
using Extended.WebApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Extended.WebApi.Filters
{
    internal sealed class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;
        public GlobalExceptionFilter(IWebHostEnvironment environment, ILogger<GlobalExceptionFilter> logger)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            ExceptionHandling(context);

            context.ExceptionHandled = true;
        }

        private static void ExceptionHandling(ExceptionContext context)
        {
            switch (context.Exception.GetBaseException())
            {
                case ValidationException exception:
                    var errors = new Dictionary<string, string[]>()
                    {
                        {exception.HResult.ToString(), new string[] {exception.Message}}
                    };
                    context.Result = ValidationResultHelper
                        .CreateValidationErrorBadRequestResponse(
                            "Please refer to the errors property for additional details.",
                            errors);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;

                case JsonPatchException exception:
                    // Need to get the friendly error message from a resources file
                    context.Result = ValidationResultHelper.CreateBadRequestResponse(exception.Message, context.ModelState);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;

                case DbException exception:
                    // Need to implement index duplication message
                    context.Result = ValidationResultHelper.CreateConflictResponse(exception.Message);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Conflict;
                    break;
                case InvalidOperationException exception:
                    context.Result = ValidationResultHelper.CreateNotFoundResponse(exception.Message);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                case Exception exception:
                    context.Result = ValidationResultHelper.CreateInternalServerErrorResponse(exception.Message);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}