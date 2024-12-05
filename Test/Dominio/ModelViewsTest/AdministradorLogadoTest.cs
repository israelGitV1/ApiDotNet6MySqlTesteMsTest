using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.ModelViews;

namespace Test.Dominio.ModelViewsTest
{
    [TestClass]
    public class AdministradorLogadoTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdiministradorLogado()
        {
            //Arrange 
            var logado = new AdministradorLogado();

            //Act
            logado.Email = "Test@gamil.com";
            logado.Perfil = Perfil.Adm.ToString();
            logado.Token = "StringToken";

            //Assert
            Assert.AreEqual("Test@gamil.com",logado.Email);
            Assert.AreEqual("Adm",logado.Perfil);
            Assert.AreEqual("StringToken",logado.Token);

        }
    }
}