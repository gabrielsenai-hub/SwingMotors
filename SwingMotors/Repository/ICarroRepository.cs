using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoFinalWestn.Models;

namespace AvaliacaoFinalWestn.Repository
{
    public interface ICarroRepository
    {
        public Task Criar(Carro carro, List<CarroImagem> Imagens);
        Task<Carro> BuscarPorIdAsync(int id);
        public Carro BuscarPorId(int id);

        public CarroImagem BuscarImgPorId(int id);

        public Task Editar(Carro carro, IEnumerable<IFormFile>? imagensNovas);
        public Task Deletar(int id);
        public Task ComprarCarro(int carroId, Usuario usuario, string PrecoPago);
        public Task<List<Carro>> Listar();
        public Task<List<CarroComprado>> Listar(string userId);
    }
}