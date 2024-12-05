using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Enuns;

namespace Test.Dominio.DTOs
{
    [TestClass]
    public class AdministradorDTOTest
    {
        [TestMethod]
        public void TestarGetSetPropriedadesAdministradorDTO()
        {
            //Arrange
            var testAdm = new AdministradorDTO();
            var testEditor = new AdministradorDTO();
            
            //Act 
            testAdm.Email = "TestAdm@gmail.com";
            testAdm.Senha = "123456";
            testAdm.Perfil = Perfil.Adm;
            
            testEditor.Email = "TestEditor@gmail.com";
            testEditor.Senha = "123456";
            testEditor.Perfil = Perfil.Editor;

            //Assert
            Assert.AreEqual("TestAdm@gmail.com",testAdm.Email);
            Assert.AreEqual("123456",testAdm.Senha);
            Assert.AreEqual("Adm",testAdm.Perfil.ToString());

            Assert.AreEqual("TestEditor@gmail.com",testEditor.Email);
            Assert.AreEqual("123456",testEditor.Senha);
            Assert.AreEqual("Editor",testEditor.Perfil.ToString());
        }
    }
}