using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

var stringMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<DbContexto>(options => 
{
  options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
});

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.MapPost("/login",(LoginDTO loginDTO) =>
{
    if(loginDTO.Email == "administrador@teste.com" && loginDTO.Senha == "123456")
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();

