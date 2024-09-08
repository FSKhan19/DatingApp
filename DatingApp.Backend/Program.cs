using DatingApp.Backend.Configs;
using DatingApp.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
