using DatingApp.Backend.Exceptions;
using DatingApp.Backend.Models;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace DatingApp.Backend.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(BusinessValidationException ex)
            {
                var error = ex.Data[ExceptionType.Business.ToString()];
                await HandleExceptionAsync(context,_env, error);
            }
            catch(CustomException ex)
            {
                var error = ExceptionFormat(ex.Message, ex.StackTrace);
                await HandleExceptionAsync(context, _env, error);
            }
            catch (Exception ex)
            {
                var error = ExceptionFormat(ex.Message, ex.StackTrace);
                await HandleExceptionAsync(context, _env, error);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, IHostEnvironment env, object error)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorMessage = env.IsDevelopment() ? error : "Internal Error Occurred";
            var response = new ResponseWrapperModel
            {
                Errors = error,
                StatusCode = (HttpStatusCode)context.Response.StatusCode
            };
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Serialize(response, jsonOptions);

            return context.Response.WriteAsync(result);
        }

        private string ExceptionFormat(string? message, string? stackTarce)
        {
            return $"{message} - {stackTarce}";
        }
    }
}
