using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

#region Builder

var builder = WebApplication.CreateBuilder(args);

var stringMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<DbContexto>(options => 
{
  options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
});

builder.Services.AddScoped<IAdministradorServico, AdministradorServico> ();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico> ();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home()));

#endregion

#region Administrador

app.MapPost("/login",([FromBody] LoginDTO loginDTO , IAdministradorServico administradorServico) =>
{
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});

#endregion

#region Veiculos

app.MapPost("/veiculos",([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico ) =>
{
  if(String.IsNullOrEmpty(veiculoDTO.Nome) && 
    String.IsNullOrEmpty(veiculoDTO.Marca) &&
    veiculoDTO.Ano > 0)
    return Results.NotFound();

  Veiculo veiculo = new Veiculo();
  veiculo.Nome = veiculoDTO.Nome;
  veiculo.Marca = veiculoDTO.Marca;
  veiculo.Ano = veiculoDTO.Ano;

  veiculoServico.Incluir(veiculo);
  return Results.Ok(veiculo);

});

app.MapGet("/veiculos",(IVeiculoServico veiculoServico ) => 
{
    var veiculos = veiculoServico.Todos();
    return Results.Ok(veiculos);
});

#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();
app.Run();

#endregion