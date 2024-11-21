using System.ComponentModel;
using System.Security.Cryptography;
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

builder.Services.AddScoped<IAdministradorServico, AdministradorServico> ();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico> ();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var stringMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<DbContexto>(options => 
{
  options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
});

var app = builder.Build();

#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administrador

ErrosDeValidacao ValidaLoginDTO (LoginDTO loginDTO)
{
  var validacao = new ErrosDeValidacao { Mensagens = new List<string>()};
  if(String.IsNullOrEmpty(loginDTO.Email))
    validacao.Mensagens.Add("Email não pode ficar em branco. ");
  if(String.IsNullOrEmpty(loginDTO.Senha))
    validacao.Mensagens.Add("Senha invalida. ");

   return validacao; 
}
ErrosDeValidacao ValidaAdministradorDTO (AdministradorDTO administradorDTO)
{
  var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };
  if(String.IsNullOrEmpty(administradorDTO.Email))
    validacao.Mensagens.Add("Email não pode ficar em branco. ");
  if(String.IsNullOrEmpty(administradorDTO.Senha))
    validacao.Mensagens.Add("Senha invalida. ");
  if(String.IsNullOrEmpty(administradorDTO.Perfil))
    validacao.Mensagens.Add("Perfil não pode ficar em branco. ");

   return validacao; 
}

app.MapPost("/login",([FromBody] LoginDTO loginDTO , IAdministradorServico administradorServico) =>
{
  var validacao =  ValidaLoginDTO(loginDTO);
  if(validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);

    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
})
.WithTags("Administradores");

app.MapPost("/administrador",([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
  var validacao =  ValidaAdministradorDTO(administradorDTO);
  if(validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);
  
  var administrador = new Administrador
  {
    Email = administradorDTO.Email,
    Senha = administradorDTO.Senha,
    Perfil = administradorDTO.Perfil,
  };
  administradorServico.Incluir(administrador);
  return Results.Created($"/administrador/{administrador.Id}",administrador);

})
.WithTags("Administradores");

app.MapDelete("/administrador",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico)=>
{
  var validacao = ValidaLoginDTO(loginDTO);
  if( validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);

  administradorServico.Excluir(loginDTO);
  return Results.NoContent();
})
.WithTags("Administradores");

app.MapGet("/administrador",([FromQuery] int? pagina ,IAdministradorServico administradorServico) =>
{
  int NumPagina = pagina > 0 ? Convert.ToInt32(pagina) : 1 ;
  var administradores = administradorServico.BuscarTodos(NumPagina);
  if(administradores == null)
    return Results.NotFound();

  return Results.Ok(administradores);
})
.WithTags("Administradores");
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

})
.WithTags("Veiculos");

app.MapGet("/veiculos",([FromQuery] int? pagina, IVeiculoServico veiculoServico ) => 
{ 
  int pg = pagina > 1 ? Convert.ToInt32(pagina) : 1;
    var veiculos = veiculoServico.Todos(pg);
    return Results.Ok(veiculos);
})
.WithTags("Veiculos");

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
})
.WithTags("Veiculos");

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

})
.WithTags("Veiculos");

app.MapDelete("veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscarPorId(id);
  if(veiculo == null)
    return Results.NotFound();
  veiculoServico.ApagarPorId(veiculo);
  return Results.NoContent();

})
.WithTags("Veiculos");
#endregion

#region App


app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion