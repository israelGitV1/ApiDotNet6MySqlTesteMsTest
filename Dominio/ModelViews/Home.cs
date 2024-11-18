using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews
{
    public class Home
    {
        public string Mensagem {get => "Bem vindo a API de Veiculos! ";}
        public string DOC {get => "/swagger";}
    }
}