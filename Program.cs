using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var stringMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<DbContexto>(options => 
{
  options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
});

app.MapGet("/", () => "Hello World!");

app.MapPost("/login",(LoginDTO loginDTO) =>
{
    if(loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();

