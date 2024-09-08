using DatingApp.Backend.Configs;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatingAppContext>(options =>
{
    // Retrieve the connection string from user secrets
    var connectionString = builder.Configuration.GetConnectionString("DatingAppContext");
    options.UseSqlite(connectionString);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp", Version = "v1" });
    c.EnableAnnotations(); // Enable annotations
});
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
// Add CORS
builder.Services.AddCors();
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
app.UseAuthorization();

app.MapControllers();

app.Run();
