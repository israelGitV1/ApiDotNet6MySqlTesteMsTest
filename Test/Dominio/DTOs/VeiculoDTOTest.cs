using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class VeiculoDTOTest
    {
        [TestMethod]
        public void TestarGetSetPropriedadesLoginDTO()
        {
            //Arrange
            var veiculoDTO = new VeiculoDTO();

            //Act
            veiculoDTO.Nome = "Corolla";
            veiculoDTO.Marca = "Toyota";
            veiculoDTO.Ano = 2022;

            //Assert
            Assert.AreEqual("Corolla",veiculoDTO.Nome);
            Assert.AreEqual("Toyota",veiculoDTO.Marca);
            Assert.AreEqual(2022,veiculoDTO.Ano);
        }
    }
}