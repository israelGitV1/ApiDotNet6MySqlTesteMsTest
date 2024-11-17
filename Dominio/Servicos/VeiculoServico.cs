using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly DbContexto _Contexto;
        public VeiculoServico (DbContexto contexto)
        {
            _Contexto = contexto;
        }
        public void ApagarPorId(Veiculo veiculo)
        {
            _Contexto.Veiculos.Remove(veiculo);
            _Contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _Contexto.Veiculos.Update(veiculo);
            _Contexto.SaveChanges();
        }

        public Veiculo? BuscarPorId(int id)
        {
           return _Contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _Contexto.Veiculos.Add(veiculo);
            _Contexto.SaveChanges();
        }

        public List<Veiculo> Todos (int pagina = 1, string? nome = null)
        {
            var query = _Contexto.Veiculos.AsQueryable();
            if(!String.IsNullOrEmpty(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(),$"%{nome}"));
            }

            int itensPorPagina = 10;
            query = query.Skip((pagina -1 ) * itensPorPagina).Take(itensPorPagina);
            return query.ToList();
        }
    }
}