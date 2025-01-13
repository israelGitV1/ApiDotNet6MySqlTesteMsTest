using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Key = configuration.GetSection("Jwt").ToString() ?? "123456";
        }

        public IConfiguration Configuration {get; set;}
        private string Key  = "";

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddAuthentication(option =>
            {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer (option => 
            {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            });

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddScoped<IAdministradorServico, AdministradorServico> ();
            services.AddScoped<IVeiculoServico, VeiculoServico> ();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    }
                    },
                    new string[] {}
                }
            });
            });

            var stringMySql = Configuration.GetConnectionString("MySql");
            services.AddDbContext<DbContexto>(options => 
            {
            options.UseMySql(stringMySql,ServerVersion.AutoDetect(stringMySql));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }
    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoint => 
            {
                #region Home
                endpoint.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
                #endregion

                #region Administrador

                string GerarTokenJwt(Administrador administrador)
                {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var clamis = new List<Claim>()
                {
                    new Claim("Email", administrador.Email),
                    new Claim("Perfil", administrador.Perfil),
                    new Claim(ClaimTypes.Role,administrador.Perfil),
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

                endpoint.MapPost("/login",([FromBody] LoginDTO loginDTO , IAdministradorServico administradorServico) =>
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

                endpoint.MapPost("/administrador",([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
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
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
                .WithTags("Administradores");

                endpoint.MapPut("/administrador",([FromQuery] string email ,string senha, [FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
                {
                var validacaoLogin = ValidaLoginDTO(new LoginDTO{Email = email,Senha = senha});
                var validacaoBodyUpdate = ValidaAdministradorDTO (administradorDTO);
                if(validacaoLogin.Mensagens.Count > 0 || validacaoBodyUpdate.Mensagens.Count > 0)
                    return Results.BadRequest(new {validacaoLogin,validacaoBodyUpdate});

                    var resultado = administradorServico.Atualizar(new LoginDTO{Email = email,Senha = senha},administradorDTO);
                    if(resultado == null)
                    return Results.NoContent();
                    return Results.Ok(resultado);  
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
                .WithTags("Administradores");

                endpoint.MapDelete("/administrador",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico)=>
                {
                var validacao = ValidaLoginDTO(loginDTO);
                if( validacao.Mensagens.Count > 0)
                    return Results.BadRequest(validacao);

                administradorServico.Excluir(loginDTO);
                return Results.NoContent();
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
                .WithTags("Administradores");

                endpoint.MapGet("/administrador",([FromQuery] int? pagina ,IAdministradorServico administradorServico) =>
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
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
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

                endpoint.MapPost("/veiculo",([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico ) =>
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
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm,Editor"})
                .WithTags("Veiculos");

                endpoint.MapGet("/veiculos",([FromQuery] int? pagina, IVeiculoServico veiculoServico ) => 
                { 
                int pg = pagina > 1 ? Convert.ToInt32(pagina) : 1;
                    var veiculos = veiculoServico.Todos(pg);
                    return Results.Ok(veiculos);
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm,Editor"})
                .WithTags("Veiculos");

                endpoint.MapGet("/veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
                {
                if(id >= 0)
                {
                    var veiculo = veiculoServico.BuscarPorId(id);
                    if (veiculo == null)
                    return Results.NoContent();
                    return Results.Ok(veiculo);
                }
                else
                    return Results.NotFound();
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm,Editor"})
                .WithTags("Veiculos");

                endpoint.MapPut("/veiculo/{id}",([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) => 
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
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
                .WithTags("Veiculos");

                endpoint.MapDelete("veiculo/{id}",([FromRoute] int id , IVeiculoServico veiculoServico) =>
                {
                var veiculo = veiculoServico.BuscarPorId(id);
                if(veiculo == null)
                    return Results.NotFound();
                veiculoServico.ApagarPorId(veiculo);
                return Results.NoContent();

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
                .WithTags("Veiculos");
                #endregion

            });

        }
    }
}