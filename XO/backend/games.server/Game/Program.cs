using Game.Services;
using GameHub.DataBaseContext;
using GameHub.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрация GameHubDbContext
builder.Services.AddDbContext<GameHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов и репозиториев
builder.Services.AddScoped<IGameHubRepository, GameHubRepository>();
builder.Services.AddScoped<IXOService, XOService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
