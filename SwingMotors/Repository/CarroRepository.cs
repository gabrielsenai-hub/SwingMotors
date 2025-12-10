using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoFinalWestn.Data;
using AvaliacaoFinalWestn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoFinalWestn.Repository
{
    public class CarroRepository : ICarroRepository
    {
        private readonly Context _context;

        public CarroRepository(Context context)
        {
            _context = context;
        }

        public Carro BuscarPorId(int id)
        {
            return _context.Carros.FirstOrDefault(c => c.Id == id);
        }
        public async Task<Carro> BuscarPorIdAsync(int id)
        {
            return await _context.Carros
                .Include(c => c.Fotos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public CarroImagem BuscarImgPorId(int id)
        {
            return _context.CarroImagens
                .FirstOrDefault(c => c.Id == id);
        }

        public async Task Criar(Carro carro, List<CarroImagem> imagens)
        {
            if (carro == null || imagens == null)
                return;

            var carroExistente = await _context.Carros
                .FirstOrDefaultAsync(n => n.Nome == carro.Nome);

            if (carroExistente == null)
            {
                carro.Fotos = imagens;
                _context.Carros.Add(carro);
            }
            else
            {
                carroExistente.Quantidade +=  carro.Quantidade;
                _context.Carros.Update(carroExistente);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(int id)
        {
            var carro = await _context.Carros
                .FirstOrDefaultAsync(c => c.Id == id);

            var imagens = await _context.CarroImagens
                .Where(i => i.CarroId == id)
                .ToListAsync();

            if (carro != null)
            {
                _context.CarroImagens.RemoveRange(imagens);
                _context.Carros.Remove(carro);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Editar(Carro carro, IEnumerable<IFormFile>? imagensNovas)
        {
            var carroB = await _context.Carros
                .Include(c => c.Fotos)
                .FirstOrDefaultAsync(c => c.Id == carro.Id);

            if (carroB == null)
                return;

            // Atualizar propriedades
            carroB.Nome = carro.Nome;
            carroB.Fabricante = carro.Fabricante;
            carroB.Motor = carro.Motor;
            carroB.Potencia = carro.Potencia;
            carroB.Torque = carro.Torque;
            carroB.ZeroaAcem = carro.ZeroaAcem;
            carroB.Quantidade = carro.Quantidade;
            carroB.VelocidadeMax = carro.VelocidadeMax;
            carroB.Transmissao = carro.Transmissao;
            carroB.Tracao = carro.Tracao;
            carroB.Preco = carro.Preco;

            // Atualizar imagens se enviadas
            if (imagensNovas != null && imagensNovas.Any())
            {
                // Remove as antigas
                _context.CarroImagens.RemoveRange(carroB.Fotos);

                // Adiciona novas
                foreach (var file in imagensNovas)
                {
                    if (file.Length == 0)
                        continue;

                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    carroB.Fotos.Add(new CarroImagem
                    {
                        CarroId = carro.Id,
                        Imagem = ms.ToArray(),
                        ContentType = file.ContentType
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Carro>> Listar()
        {
            return await _context.Carros
                .Include(c => c.Fotos) // se tiver navigation property
                .ToListAsync();
        }
        public async Task ComprarCarro(int carroId, Usuario usuario, string PrecoPago)
        {
            var carro = await _context.Carros
                .Include(c => c.Fotos)
                .FirstOrDefaultAsync(c => c.Id == carroId);

            // diminui do estoque
            carro.Quantidade--;

            // registra a compra
            var compra = new CarroComprado
            {
                UsuarioId = usuario.Id,
                CarroId = carroId,
                PrecoPago = PrecoPago
            };

            _context.CarrosComprados.Add(compra);

            await _context.SaveChangesAsync();
        }

        public async Task<List<CarroComprado>> Listar(string userId)
        {
            return await _context.CarrosComprados
                .Where(cc => cc.UsuarioId == userId)
                .Include(cc => cc.Carro)
                .ThenInclude(c => c.Fotos)
                .ToListAsync();
        }
    }
}