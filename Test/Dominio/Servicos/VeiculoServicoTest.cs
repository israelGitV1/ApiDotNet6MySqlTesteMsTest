using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Dominio.Servicos
{
    [TestClass]
    public class VeiculoServicoTest
    {
        private DbContexto CriacaoContextoTest ()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "","..","..",".."));
            var builder = new ConfigurationBuilder()
                              .SetBasePath(path ?? Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json",optional:false, reloadOnChange: true)
                              .AddEnvironmentVariables();
            var configuration = builder.Build();
            return new DbContexto(configuration);
        }

        [TestMethod]
        public void TestVeiculoServicoIncluir()
        {
            //Arrange
            var contexto = CriacaoContextoTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");
            var veiculoServico = new VeiculoServico(contexto);
            var veiculo = new Veiculo
            {
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2023
            };

            //Act
            veiculoServico.Incluir(veiculo);
        
            //Assert
            Assert.AreEqual(1 , contexto.Veiculos.Count());
            Assert.AreEqual(veiculo.Id, contexto.Veiculos.Find(veiculo.Id).Id );
            Assert.AreEqual(veiculo.Nome, contexto.Veiculos.Find(veiculo.Id).Nome);
            Assert.AreEqual(veiculo.Marca, contexto.Veiculos.Find(veiculo.Id).Marca);
            Assert.AreEqual(veiculo.Ano, contexto.Veiculos.Find(veiculo.Id).Ano);
            
        }
        
        [TestMethod]
        public void TestVeiculoServicoAtualizar()
        {
            //Arrange
            var contexto = CriacaoContextoTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");
            var veiculoServico = new VeiculoServico(contexto);
            var veiculo = new Veiculo
            {
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2023
            };
            var veiculoAtualizado = new Veiculo
            {
                Nome = "Corolla2",
                Marca = "Toyota2",
                Ano = 2022
            };

            //Act
            veiculoServico.Incluir(veiculo);

            var veiculoDb = contexto.Veiculos.Find(veiculo.Id);
            veiculoDb.Nome = veiculoAtualizado.Nome;
            veiculoDb.Marca = veiculoAtualizado.Marca;
            veiculoDb.Ano = veiculoAtualizado.Ano;

            veiculoServico.Atualizar(veiculoDb);
        
            //Assert
            Assert.AreEqual(1 , contexto.Veiculos.Count());
            Assert.AreEqual(veiculo.Id, veiculoDb.Id);
            Assert.AreEqual(veiculoAtualizado.Nome, contexto.Veiculos.Find(veiculo.Id).Nome);
            Assert.AreEqual(veiculoAtualizado.Marca, contexto.Veiculos.Find(veiculo.Id).Marca);
            Assert.AreEqual(veiculoAtualizado.Ano, contexto.Veiculos.Find(veiculo.Id).Ano);
            
        }
        
        [TestMethod]
        public void TestVeiculoServicoApagarPorId()
        {
            //Arrange
            var contexto = CriacaoContextoTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");
            var veiculoServico = new VeiculoServico(contexto);
            var veiculo = new Veiculo
            {
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2023
            };

            //Act
            veiculoServico.Incluir(veiculo);
            veiculoServico.ApagarPorId(veiculo);

            //Assert
            Assert.AreEqual(0 , contexto.Veiculos.Count());
            
        }
        
        [TestMethod]
        public void TestVeiculoServicoBuscarPorId()
        {
            //Arrange
            var contexto = CriacaoContextoTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");
            var veiculoServico = new VeiculoServico(contexto);
            var veiculoList = new List<Veiculo>();
            for(var i = 0;i < 10; i++)
                veiculoList.Add(new Veiculo{Nome = $"Corolla{i+1}",Marca = $"Toyota{i+1}",Ano = 2023});

            //Act
            for(var i = 0;i < 10; i++)
            veiculoServico.Incluir(veiculoList[i]);

            var veiculoDb = veiculoServico.BuscarPorId(veiculoList[5].Id) ?? new Veiculo();
            
            //Assert
            Assert.AreEqual(veiculoList[5].Id , veiculoDb.Id);
            Assert.AreEqual(veiculoList[5].Nome , veiculoDb.Nome);
            Assert.AreEqual(veiculoList[5].Marca, veiculoDb.Marca);
            Assert.AreEqual(veiculoList[5].Ano, veiculoDb.Ano);
            
            
        }
        
        [TestMethod]
        public void TestVeiculoServicoTodos()
        {
            //Arrange
            var contexto = CriacaoContextoTest();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");
            var veiculoServico = new VeiculoServico(contexto);
            var veiculoList = new List<Veiculo>();
            for(var i = 0;i < 40; i++)
                veiculoList.Add(new Veiculo{Nome = $"Corolla{i+1}",Marca = $"Toyota{i+1}",Ano = 2023});

            //Act
            for(var i = 0;i < 40; i++)
            veiculoServico.Incluir(veiculoList[i]);

            var BuscaPagina1 = veiculoServico.Todos(1) ?? new List<Veiculo>();
            var BuscaPagina2 = veiculoServico.Todos(2) ?? new List<Veiculo>();
            var BuscaPagina3 = veiculoServico.Todos(3) ?? new List<Veiculo>();
            
            
            //Assert
            Assert.AreEqual(10 , BuscaPagina1.Count());
            Assert.AreEqual(1,BuscaPagina1[0].Id);
            Assert.AreEqual(10,BuscaPagina1[9].Id);

            Assert.AreEqual(10 , BuscaPagina2.Count());
            Assert.AreEqual(11,BuscaPagina2[0].Id);
            Assert.AreEqual(20,BuscaPagina2[9].Id);

            Assert.AreEqual(10 , BuscaPagina2.Count());
            Assert.AreEqual(21,BuscaPagina3[0].Id);
            Assert.AreEqual(30,BuscaPagina3[9].Id);
            
            
        }

    }
}