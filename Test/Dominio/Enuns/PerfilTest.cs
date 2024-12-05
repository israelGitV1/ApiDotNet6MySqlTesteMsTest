using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Enuns;

namespace Test.Dominio.Enuns
{
    [TestClass]
    public class PerfilTest
    {
        [TestMethod]
        public void TestarEnunPerfilRetornandoString()
        {
            //Arrange
            var adm = Perfil.Adm;
            var editor = Perfil.Editor;
            string admString,editorString;

            //Act
            admString = adm.ToString();
            editorString = editor.ToString();

            //Assert
            Assert.AreEqual("Adm", admString);
            Assert.AreEqual("Editor", editorString);
        }
    }
}