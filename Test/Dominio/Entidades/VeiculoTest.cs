using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entidades;

namespace Test.Dominio.Entidades
{
    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesVeiculo()
        {   
            //Arrange
            var veiculo = new Veiculo();

            //Act
            veiculo.Id = 1;
            veiculo.Nome = "Corolla";
            veiculo.Marca = "Toyota";
            veiculo.Ano = 2024;        

            //Assert
            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("Corolla", veiculo.Nome);
            Assert.AreEqual("Toyota", veiculo.Marca);
            Assert.AreEqual(2024, veiculo.Ano);
        }
    }
}