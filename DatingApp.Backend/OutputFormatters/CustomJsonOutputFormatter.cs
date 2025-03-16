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

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = new ResponseWrapperModel
            {
                StatusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode,
            };

            // Automatically detect errors and assign them
            if (context.Object is ValidationProblemDetails validationErrors)
            {
                response.Errors = validationErrors.Errors.SelectMany(kvp => kvp.Value).ToList();
            }
            else if (context.Object is ProblemDetails problemDetails)
            {
                response.Errors = problemDetails.Title ?? "An error occurred.";
            }
            else if ((int)response.StatusCode >= 400) // Handle all 4xx and 5xx errors dynamically
            {
                response.Errors = context.Object;
            }
            else
            {
                response.Success = true;
                response.Data = context.Object;
            }

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, jsonOptions);

            return context.HttpContext.Response.WriteAsync(json);
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return context.ObjectType != null && !context.ObjectType.IsPrimitive;
        }
    }
}
