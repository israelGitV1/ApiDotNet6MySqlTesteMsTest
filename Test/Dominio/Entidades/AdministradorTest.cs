using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entidades;

namespace Test.Dominio.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arrange
            var adm = new Administrador();

            // Act
            adm.Id = 1;
            adm.Email = "Test@test.com";
            adm.Senha = "Teste";
            adm.Perfil = "Adm";

            // Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("Test@test.com", adm.Email);
            Assert.AreEqual("Teste", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}