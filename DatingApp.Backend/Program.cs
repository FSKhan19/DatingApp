using DatingApp.Backend.Consts;
using DatingApp.Backend.Data;
using DatingApp.Backend.Services;
using DatingApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DatingApp.Backend.Extensions;
using DatingApp.Backend.Middlewares;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using DatingApp.Backend.OutputFormatters;
using DatingApp.Backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using DatingApp.Backend.Configs.Mapping;

var builder = WebApplication.CreateBuilder(args);

// App Configurations
builder.Services.AddConfigurations(builder.Configuration);

// App Configurations
builder.Services.AddDatabase(builder.Configuration);

// App Services
builder.Services.AddApplicationServices();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        return new BadRequestObjectResult(actionContext.ModelState);
    };
});

// Add FluentValidation with automatic registration and auto-validation
builder.Services.AddFluentValidationWithAutoRegistration(Assembly.GetExecutingAssembly());

// Add AutoMapper
builder.Services.AddMapsterConfig();

// Add CORS
builder.Services.AddCors();

// Add Swagger
builder.Services.AddSwaggerServices();

builder.Services.AddControllers(options =>
{
    // Add your custom output formatter at the beginning of the list
    options.OutputFormatters.Insert(0, new CustomJsonOutputFormatter());
});

// JWT and Identity Services
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Apply database migrations and seed data
await app.ApplySeedDataAsync();

#region HTTP request pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingApp v1"));
}

// Register exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();  // Exception handling middleware should be first

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Configure CORS
app.UseCors(c =>
{
    c.AllowAnyHeader().AllowAnyMethod().WithOrigins(App.FRONT_END_BASE_URL);
});

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();
#endregion

app.Run();
