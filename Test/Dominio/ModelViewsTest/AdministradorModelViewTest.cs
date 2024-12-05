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
    public class AdministradorModelViewTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesAdiministradorModelView()
        {
            //Arrange
            var administradorModelView = new AdministradorModelView();

            //Act
            administradorModelView.Id = 1;
            administradorModelView.Email = "Test@gmail.com";
            administradorModelView.Perfil = Perfil.Editor.ToString();

            //Assert
            Assert.AreEqual(1,administradorModelView.Id);
            Assert.AreEqual("Test@gmail.com",administradorModelView.Email);
            Assert.AreEqual("Editor",administradorModelView.Perfil);
        }
    }
}