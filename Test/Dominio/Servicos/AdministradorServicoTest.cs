using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Dominio.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto CriarContextoDeTest()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "","..","..",".."));
            
            // configurar o configuration builder
            var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json",optional:false, reloadOnChange:true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return new DbContexto(configuration);
        }
        [TestMethod]
        public void TestAdministradorServicoIncluir()
        {
           //Arrange 

           var adm = new Administrador{
            Email = "test@gmail.com",
            Senha = "123456",
            Perfil = "Adm"
           };
           var contexto = CriarContextoDeTest();
           contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
           var servicos = new AdministradorServico(contexto);

           //Act
           servicos.Incluir(adm);

           //Assert
           Assert.AreEqual(adm.Id,contexto.Administradores.Find(adm.Id).Id);
           Assert.AreEqual(adm.Email,contexto.Administradores.Find(adm.Id).Email);
           Assert.AreEqual(adm.Senha,contexto.Administradores.Find(adm.Id).Senha);
           Assert.AreEqual(adm.Perfil,contexto.Administradores.Find(adm.Id).Perfil);
        }

        [TestMethod]
        public void TestAdministradorServicoLogin ()
        {
            //Arrange  
            var adm = new Administrador{
            Email = "testLogin@gmail.com",
            Senha = "123456",
            Perfil = "Adm"
           };
            var login = new LoginDTO {  
                Email = "TestLogin@gmail.com",
                Senha = "123456",
            };
            var contexto = CriarContextoDeTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var servicos = new AdministradorServico(contexto);
        
            servicos.Incluir(adm); // incluindo adm do login 

            //Act
            var resultdo = servicos.Login(login);
         
            //Assert
            Assert.IsTrue(resultdo != null);
            Assert.AreEqual(adm.Email,resultdo.Email);
            Assert.AreEqual(adm.Senha,resultdo.Senha);
            Assert.AreEqual(adm.Perfil,resultdo.Perfil);


        }
        
        [TestMethod]
        public void TestAdministradorServicoAtualizar()
        {
            //Arrange 
            var adm = new Administrador{
            Email = "test@gmail.com",
            Senha = "123456",
            Perfil = "Adm"
           };
            var login = new LoginDTO {  
                Email = "Test@gmail.com",
                Senha = "123456",
            };
            var novoAdm = new AdministradorDTO
            {
                Email = "TestAtualizado@gamil.com",
                Senha = "1234567",
                Perfil = MinimalApi.Dominio.Enuns.Perfil.Adm
            };

            var contexto = CriarContextoDeTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var servicos = new AdministradorServico(contexto);
            servicos.Incluir(adm);

            //Act
            var admAtualizado = servicos.Atualizar(login,novoAdm);

            //Assert
            Assert.AreEqual(adm.Id,admAtualizado.Id);
            Assert.AreEqual(novoAdm.Email,admAtualizado.Email);
            Assert.AreEqual(novoAdm.Senha,admAtualizado.Senha);
            Assert.AreEqual(novoAdm.Perfil.ToString(),admAtualizado.Perfil);
        }

        [TestMethod]
        public void TestAdministradorServicoExluir()
        {
            //Arrange 
            var adm = new Administrador{
            Email = "testExluir@gmail.com",
            Senha = "123456",
            Perfil = "Adm"
           };
            var login = new LoginDTO {  
                Email = "TestExluir@gmail.com",
                Senha = "123456",
            };

            var contexto = CriarContextoDeTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var servicos = new AdministradorServico(contexto);
            servicos.Incluir(adm);

            //Act
            var admExluido = servicos.Excluir(login);

            //Assert
            Assert.IsTrue(admExluido);
        }

        [TestMethod]
        public void TestAdministradorServicoBuscarTodos()
        {
            //Arrange 
            var admAddLista = new List<Administrador>();
            for(var i = 0; i < 30; i++)
            {
                admAddLista.Add(
                new Administrador
                {
                Email = $"test{i+1}@gmail.com",
                Senha = "123456",
                Perfil = "Adm"
                }
                );
            }
            var contexto = CriarContextoDeTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var servicos = new AdministradorServico(contexto);
            for(var i = 0; i< 30; i++)
            servicos.Incluir(admAddLista[i]);

            //Act
            var admListaPagina1 = servicos.BuscarTodos(1);
            var admListaPagina2 = servicos.BuscarTodos(2);
            var admListaPagina3 = servicos.BuscarTodos(3);

            //Assert
            Assert.AreEqual(10,admListaPagina1.Count()); 
            Assert.AreEqual(1,admListaPagina1[0].Id);
            Assert.AreEqual(10,admListaPagina1[9].Id);

            Assert.AreEqual(10,admListaPagina2.Count());
            Assert.AreEqual(11,admListaPagina2[0].Id);
            Assert.AreEqual(20,admListaPagina2[9].Id);

            Assert.AreEqual(10,admListaPagina3.Count());
            Assert.AreEqual(21,admListaPagina3[0].Id);
            Assert.AreEqual(30,admListaPagina3[9].Id);
        }

        [TestMethod]
        public void TestAdministradorServicoBuscarPorId()
        {
            var admAddLista = new List<Administrador>();
            for(var i = 0; i < 10; i++)
            {
                admAddLista.Add(
                new Administrador
                {
                Email = $"test{i+1}@gmail.com",
                Senha = "123456",
                Perfil = "Adm"
                }
                );
            }
            var contexto = CriarContextoDeTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var servicos = new AdministradorServico(contexto);

            for(var i = 0; i< 10; i++)
            servicos.Incluir(admAddLista[i]);

            //Act
            var admLista = servicos.BuscarPorId(6);

            //Assert
            Assert.AreEqual(6,admLista.Id);
        }

    }
}