using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

#region Builder

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if(string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
  option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer (option => 
{
  option.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
    ValidateIssuer = false,
    ValidateAudience = false,
  };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServico> ();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico> ();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var stringMySql = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<DbContexto>(options => 
{
  options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
});
#endregion

#region App
var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administrador

string GerarTokenJwt(Administrador administrador)
{
  var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
  var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

  var clamis = new List<Claim>()
  {
    new Claim("Email", administrador.Email),
    new Claim("Perfil", administrador.Perfil),
  };
  var token = new JwtSecurityToken(
    claims : clamis,
    expires : DateTime.Now.AddDays(1),
    signingCredentials : credentials  
  );

  return new JwtSecurityTokenHandler().WriteToken(token);
} 

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
  if(administradorDTO.Perfil != Perfil.Adm && administradorDTO.Perfil != Perfil.Editor) 
    validacao.Mensagens.Add("Perfin invalido !(Digite 0 para Administrador OU 1 para Editor)");

   return validacao; 
}

app.MapPost("/login",([FromBody] LoginDTO loginDTO , IAdministradorServico administradorServico) =>
{
  var validacao =  ValidaLoginDTO(loginDTO);
  if(validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);

  var administrador = administradorServico.Login(loginDTO);

    if(administrador != null)
    {
        string token = GerarTokenJwt(administrador);
        return Results.Ok(new AdministradorLogado
        {
          Email = administrador.Email,
          Perfil = administrador.Perfil,
          Token = token,
        });
    }
    else
        return Results.Unauthorized();
})
.AllowAnonymous()
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
    Perfil = administradorDTO.Perfil.ToString()
  };
  administradorServico.Incluir(administrador);
  return Results.Created($"/administrador/{administrador.Id}",new AdministradorModelView
  {
    Id = administrador.Id,
    Email = administrador.Email,
    Perfil = administrador.Perfil,
  });

})
//.RequireAuthorization()
.RequireAuthorization().WithTags("Administradores");

app.MapDelete("/administrador",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico)=>
{
  var validacao = ValidaLoginDTO(loginDTO);
  if( validacao.Mensagens.Count > 0)
    return Results.BadRequest(validacao);

  administradorServico.Excluir(loginDTO);
  return Results.NoContent();
})
.RequireAuthorization().WithTags("Administradores");

app.MapGet("/administrador",([FromQuery] int? pagina ,IAdministradorServico administradorServico) =>
{
  int NumPagina = pagina > 0 ? Convert.ToInt32(pagina) : 1 ;
  var administradores = administradorServico.BuscarTodos(NumPagina);
  if(administradores == null)
    return Results.NotFound();

  var administradorModelView = new List<AdministradorModelView>();
  foreach( var administrador in administradores)
  {
    administradorModelView.Add(new AdministradorModelView{
      Id = administrador.Id,
      Email = administrador.Email,
      Perfil = administrador.Perfil,
    });
  }
  return Results.Ok(administradorModelView);
})
.RequireAuthorization().WithTags("Administradores");
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
.RequireAuthorization().WithTags("Veiculos");

app.MapGet("/veiculos",([FromQuery] int? pagina, IVeiculoServico veiculoServico ) => 
{ 
  int pg = pagina > 1 ? Convert.ToInt32(pagina) : 1;
    var veiculos = veiculoServico.Todos(pg);
    return Results.Ok(veiculos);
})
.RequireAuthorization().WithTags("Veiculos");

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
.RequireAuthorization().WithTags("Veiculos");

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
.RequireAuthorization().WithTags("Veiculos");

app.MapDelete("veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscarPorId(id);
  if(veiculo == null)
    return Results.NotFound();
  veiculoServico.ApagarPorId(veiculo);
  return Results.NoContent();

})
.RequireAuthorization().WithTags("Veiculos");
#endregion


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion