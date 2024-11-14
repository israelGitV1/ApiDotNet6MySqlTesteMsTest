using Microsoft.Extensions.FileProviders;
using MinimalApi.Dominio.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
List<ContaDTO> conta = new List<ContaDTO>();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login",(LoginDTO loginDTO) =>
{
    if(loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});

app.MapPost("/cadastro",(ContaDTO contaDTO) =>
{
  conta.Add(contaDTO);
  return Results.Ok("Cadastro com Sucesso!");
});
app.MapGet("/contas",() =>
{
  if(conta.Count > 0)
    return Results.Ok(conta.ToList());
  else
    return Results.NoContent();
});

app.Run();

public class ContaDTO
{
    public string Nome {get; set;} = default!;
    public string Email {get; set;} = default!;

    public string Senha {get; set;} = default!;
}
