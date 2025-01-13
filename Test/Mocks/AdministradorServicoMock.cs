using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;

namespace Test.Mocks
{
    public class AdministradorServicoMock : IAdministradorServico
    {
        
        private static List<Administrador> administradores = new List<Administrador>(){
            new Administrador{
                Id = 1,
                Email = "adm@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            },
            new Administrador{
                Id = 2,
                Email = "editor@teste.com",
                Senha = "123456",
                Perfil = "Editor"
            },
        };

        public Administrador? Atualizar(LoginDTO loginDTO, AdministradorDTO administradorDTO)
        {
            var administrador = administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
            if(administrador == null )
            return administrador;

            administradores[administrador.Id].Email = administradorDTO.Email;
            administradores[administrador.Id].Senha = administradorDTO.Senha;
            administradores[administrador.Id].Perfil = administradorDTO.Perfil.ToString();
            return administradores.Find(a => a.Id == administrador.Id);
        }

        public Administrador? BuscarPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public List<Administrador>? BuscarTodos(int pagina)
        {
            return administradores;
        }

        public bool Excluir(LoginDTO loginDTO)
        {
            var administrador = administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
            if(administrador == null)
            return false;
            
            return administradores.Remove(administrador);
        }

        public void Incluir(Administrador administrador)
        {
            administradores.Add(administrador);
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            return administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
        }
    }
    
}