using DatingApp.Backend.Configs;
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

var builder = WebApplication.CreateBuilder(args);
// App Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers(options =>
{
    // Add your custom output formatter at the beginning of the list
    options.OutputFormatters.Insert(0, new CustomJsonOutputFormatter());
});
builder.Services.AddSwaggerServices();
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
// Add CORS
builder.Services.AddCors();
// JWT
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c =>
{
    c.AllowAnyHeader().AllowAnyMethod().WithOrigins(App.FRONT_END_BASE_URL);
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Register exception handling middleware

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DatingAppContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
