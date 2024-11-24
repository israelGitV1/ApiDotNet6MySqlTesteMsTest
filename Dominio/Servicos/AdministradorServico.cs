using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _Contexto;

        public AdministradorServico (DbContexto contexto)
        {
            _Contexto = contexto;
        }

        public void Incluir(Administrador administrador)
        {
            _Contexto.Add(administrador);
            _Contexto.SaveChanges();
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _Contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }

        public List<Administrador>? BuscarTodos (int pagina = 1 )
        {
            var administradores = _Contexto.Administradores.ToList();
            var numeroPorPagina = 10;
            var administradoresPagina = administradores.Skip((pagina-1)*numeroPorPagina).Take(numeroPorPagina).ToList();

            return administradoresPagina;
        }

        public bool Excluir(LoginDTO loginDTO)
        {
            var administrador = _Contexto.Administradores.Where(x => x.Email == loginDTO.Email && x.Senha == loginDTO.Senha).FirstOrDefault();
            if(administrador != null)
            {
                _Contexto.Administradores.Remove(administrador);
                _Contexto.SaveChanges();
                return true;
            }
                return false;
        }

        public Administrador? Atualizar(LoginDTO loginDTO,AdministradorDTO administradorDTO)
        {

            var admAtual = _Contexto.Administradores.Where(adm => adm.Email == loginDTO.Email && adm.Senha == loginDTO.Senha).FirstOrDefault();
            if(admAtual == null)
             return admAtual;
             admAtual.Email = administradorDTO.Email;
             admAtual.Senha = administradorDTO.Senha;
             admAtual.Perfil = administradorDTO.Perfil.ToString();

            _Contexto.Administradores.Update(admAtual);
            _Contexto.SaveChanges();
            
            return admAtual;
        }
    }
}