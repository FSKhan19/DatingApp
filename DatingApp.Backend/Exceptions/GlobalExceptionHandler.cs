using DatingApp.Backend.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace DatingApp.Backend.Exceptions
{

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(IHostEnvironment env)
        {
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Set default response content type
            httpContext.Response.ContentType = "application/json";

            // Handle specific exception types
            object error;
            HttpStatusCode statusCode;
            bool isDevEnv = _env.IsDevelopment();

            switch (exception)
            {
                case BusinessValidationException businessValidationEx:
                    // Retrieve the validation errors from the Data dictionary
                    error = businessValidationEx.Data[ExceptionType.Business.ToString()];
                    statusCode = HttpStatusCode.BadRequest; // 400 for validation errors
                    break;

                case CustomException customEx:
                    error = ExceptionFormat(isDevEnv, customEx.Message, customEx.StackTrace ?? string.Empty);
                    statusCode = HttpStatusCode.InternalServerError; // 500 for custom exceptions
                    break;

                default:
                    // Handle all other exceptions
                    error = _env.IsDevelopment()
                        ? ExceptionFormat(isDevEnv, exception.Message, exception.StackTrace ?? string.Empty)
                        : "Internal Error Occurred";
                    statusCode = HttpStatusCode.InternalServerError; // 500 for generic errors
                    break;
            }

            // Write the error response
            await WriteErrorResponse(httpContext, statusCode, error ?? string.Empty, cancellationToken);

            return true; // Indicates the exception was handled
        }

        private async Task WriteErrorResponse(HttpContext httpContext, HttpStatusCode statusCode, object error, CancellationToken cancellationToken)
        {
            // Set the HTTP status code
            httpContext.Response.StatusCode = (int)statusCode;

            // Create the standardized response model
            var response = new ResponseWrapperModel
            {
                Errors = error,
                StatusCode = statusCode
            };

            // Serialize the response to JSON
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var result = JsonSerializer.Serialize(response, jsonOptions);

            // Write the JSON response to the HTTP context
            await httpContext.Response.WriteAsync(result, cancellationToken);
        }

        private string ExceptionFormat(bool isDevEnv, string message = "", string stackTrace = "")
        {
            return isDevEnv ?  $"{message} - {stackTrace}" : $"{message}";
        }
    }
}
