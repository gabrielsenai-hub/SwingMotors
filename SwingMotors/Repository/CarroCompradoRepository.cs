using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoFinalWestn.Data;
using AvaliacaoFinalWestn.Models;
using Microsoft.EntityFrameworkCore;  

namespace AvaliacaoFinalWestn.Repository
{
    public class CarroCompradoRepository : ICarroCompradoRepository
    {
        private readonly Context _context;

        public CarroCompradoRepository(Context context)
        {
            _context = context;
        }

        public async Task<CarroComprado> BuscarCompraCompletaAsync(int compraId)
        {
            return await _context.CarrosComprados
                .Include(c => c.Carro)
                    .ThenInclude(c => c.Fotos)
                .FirstOrDefaultAsync(c => c.Id == compraId);
        }
    }
}
