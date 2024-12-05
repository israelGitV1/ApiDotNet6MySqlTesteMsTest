using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.ModelViews;

namespace Test.Dominio.ModelViewsTest
{
    [TestClass]
    public class HomeTest
    {
        [TestMethod]
        public void TestGetPropriedadesMensagemDeHome()
        {
            //arrange
            var home = new Home();
            string mensagem,doc;

            //Act
            mensagem = home.Mensagem;
            doc = home.DOC;

            //Assert
            Assert.AreEqual("Bem vindo a API de Veiculos! ",mensagem);
            Assert.AreEqual("/swagger",doc);
        
        }
    }
}