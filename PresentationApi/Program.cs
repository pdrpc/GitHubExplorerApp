using Domain.Interfaces;
using Domain.Services;
using Application.Interfaces;
using Application.Services;
using Infrastructure.GitHub.Services; 
using Infrastructure.Persistance;
using PresentationApi.Middleware;


var builder = WebApplication.CreateBuilder(args);

// 1. Configurações da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt => opt.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader()));

// 2. Registro das Camadas (Injeção de Dependência)
builder.Services.AddScoped<IRelevanceService, RelevanceService>();
builder.Services.AddScoped<IGitHubAppService, Application.Services.GitHubAppService>();
builder.Services.AddSingleton<IFavoriteRepository, InMemoryFavoriteRepository>();

builder.Services.AddHttpClient<IGithubService, Infrastructure.GitHub.Services.GitHubService>(client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "GitHubExplorerApp");
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// 3. Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();