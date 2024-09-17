using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Text;
using DatingApp.Backend.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mime;


namespace DatingApp.Backend.OutputFormatters
{
    public class CustomJsonOutputFormatter : TextOutputFormatter
    {
        public CustomJsonOutputFormatter()
        {
            // Define supported media types
            SupportedMediaTypes.Add(MediaTypeNames.Application.Json);

            // Add supported encodings
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
                                                    Encoding selectedEncoding)
        {
            var response = new ResponseWrapperModel()
            {
                StatusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode,
            };
            object? data = context.Object;
            if (data is ValidationProblemDetails validationErrors)
                response.Errors = TransformValidationProblemDetailsIntoErrors(validationErrors);
            else if (data is ProblemDetails problemDetails)
                response.Errors = problemDetails.Title;
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                response.Errors = data;
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                response.Errors = data;
            else if (response.StatusCode == HttpStatusCode.NotFound)
                response.Errors = data;
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                response.Errors = data;

            if (response.Errors is null && response.Data is null)
            {
                response.Success = true;
                response.Data = data;
            }
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var json = JsonSerializer.Serialize(response, jsonOptions);

            return context.HttpContext.Response.WriteAsync(json);
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            // Check if the response type is supported
            return context.ObjectType == typeof(object) || context.ObjectType.IsClass;
        }


        private object? TransformValidationProblemDetailsIntoErrors(ValidationProblemDetails validation)
        {
            var errorMessageList = new List<string>();
            foreach (var kvp in validation.Errors)
            {
                var messages = kvp.Value as IEnumerable<string>;
                if (messages != null)
                {
                    errorMessageList.AddRange(messages.Select(x => x).ToList());
                }
            }

            return errorMessageList;
        }


    }
}
