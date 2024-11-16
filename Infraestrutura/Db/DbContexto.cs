using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration  _ConfiguracaoAppSettings;

        public DbContexto (IConfiguration configurationAppSettings)
        {
            _ConfiguracaoAppSettings = configurationAppSettings;
        }

        public DbSet<Administrador> Administradores {get; set;} = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            if(!optionsBuilder.IsConfigured)
            {
                var stringConexao = _ConfiguracaoAppSettings.GetConnectionString("MySql")?.ToString();

                if(!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseMySql(stringConexao,ServerVersion.AutoDetect(stringConexao));
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Administrador> ().HasData( new Administrador
            {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            });
        }

    }
}