using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class LoginDTOTest
    {
        [TestMethod]
        public void TestarGetSetPropriedadesLoginDTO()
        {
            //Arrange
            var loginDTO = new LoginDTO();

            //Act
            loginDTO.Email = "Test@gmail.com";
            loginDTO.Senha = "123456";

            //Assert
            Assert.AreEqual("Test@gmail.com",loginDTO.Email);
            Assert.AreEqual("123456",loginDTO.Senha);
        }
    }
}