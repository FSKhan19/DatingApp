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

var builder = WebApplication.CreateBuilder(args);
// App Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
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

app.MapControllers();

app.Run();
