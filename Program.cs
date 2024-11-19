using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
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

ErrosDeValidacao ValidaDTO (VeiculoDTO veiculoDTO)
{
  var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };
  
  if(String.IsNullOrEmpty(veiculoDTO.Nome))
    validacao.Mensagens.Add("O nome não pode ser vazio. ");
    
  if(String.IsNullOrEmpty(veiculoDTO.Marca))
    validacao.Mensagens.Add("O marca não pode ficar em branco. ");
    
  if(veiculoDTO.Ano < 1950)
    validacao.Mensagens.Add("Veiculo muito antigo aceito somente anos superiores a 1950. ");
    

  return validacao;
}

app.MapPost("/veiculo",([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico ) =>
{
  var validacao = ValidaDTO(veiculoDTO);  
  if(validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);

  Veiculo veiculo = new Veiculo
  {
    Nome = veiculoDTO.Nome,
    Marca = veiculoDTO.Marca,
    Ano = veiculoDTO.Ano,
  };

  veiculoServico.Incluir(veiculo);
  return Results.Created($"/veiculo/{veiculo.Id}",veiculo);

});

app.MapGet("/veiculos",([FromQuery] int? pagina, IVeiculoServico veiculoServico ) => 
{ 
  int pg = pagina > 1 ? Convert.ToInt32(pagina) : 1;
    var veiculos = veiculoServico.Todos(pg);
    return Results.Ok(veiculos);
}).WithTags("veiculos");

app.MapGet("/veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
{
  if(id >= 0)
  {
    var veiculo = veiculoServico.BuscarPorId(id);
    if (veiculo == null)
      return Results.NotFound();
    return Results.Ok(veiculo);
  }
  else
    return Results.NotFound();
}).WithTags("veiculos");

app.MapPut("/veiculo/{id}",([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) => 
{

  var veiculo = veiculoServico.BuscarPorId(id);
  if(veiculo == null)
    return Results.NotFound();

  var validacao = ValidaDTO(veiculoDTO);
  if(validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);  

  veiculo.Nome = veiculoDTO.Nome;
  veiculo.Marca = veiculoDTO.Marca;
  veiculo.Ano = veiculoDTO.Ano;

  veiculoServico.Atualizar(veiculo);
  return Results.Ok(veiculo);

}).WithTags("veiculos");

app.MapDelete("veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscarPorId(id);
  if(veiculo == null)
    return Results.NotFound();
  veiculoServico.ApagarPorId(veiculo);
  return Results.NoContent();

}).WithTags("veiculos");
#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();
app.Run();

#endregion