using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.ModelViews;

namespace Test.Dominio.ModelViewsTest
{
    [TestClass]
    public class ErrosDeValidacaoTest
    {
        [TestMethod]
        public void TestGetSetPropriedadesErrosDeValidacao()
        {
            //Arrange
            var mensagensErros = new ErrosDeValidacao();

            //Act
            mensagensErros.Mensagens = new List<string>();
            mensagensErros.Mensagens.Add("Mensagem de Test 1");
            mensagensErros.Mensagens.Add("Mensagem de Test 2");
            mensagensErros.Mensagens.Add("Mensagem de Test 3");
            var test = mensagensErros.Mensagens[2];

            //Assert
            Assert.AreEqual(3,mensagensErros.Mensagens.Count());
            Assert.AreEqual("Mensagem de Test 1",mensagensErros.Mensagens[0]);
            Assert.AreEqual("Mensagem de Test 2",mensagensErros.Mensagens[1]);
            Assert.AreEqual("Mensagem de Test 3",mensagensErros.Mensagens[2]);
        }
    }
}