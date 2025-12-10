using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoFinalWestn.Models;

namespace AvaliacaoFinalWestn.Repository
{
    public interface ICarroCompradoRepository
    {
        public Task<CarroComprado> BuscarCompraCompletaAsync(int compraId);

    }
}