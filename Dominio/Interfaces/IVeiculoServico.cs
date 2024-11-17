using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        public void Incluir (Veiculo veiculo);
        public void Atualizar (Veiculo veiculo);
        public void ApagarPorId (Veiculo veiculo);
        public Veiculo? BuscarPorId (int id);
        public List<Veiculo> Todos (int pagina = 1, string? nome = null);

    }

}