using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.ModelViews;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }
        
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestarMockLogadoEGerarOToken()
        {
            //Arrange
            var loginDTO = new LoginDTO{
                Email = "adm@teste.com",
                Senha = "123456",
            };
            
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

            //Act
            var response = await Setup.client.PostAsync("/login", content);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(admLogado?.Email ?? "");
            Assert.IsNotNull(admLogado?.Perfil ?? "");
            Assert.IsNotNull(admLogado?.Token ?? "");

            Console.WriteLine(admLogado?.Token);
        }
    }
}