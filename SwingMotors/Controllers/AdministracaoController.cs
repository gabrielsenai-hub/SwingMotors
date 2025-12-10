using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AvaliacaoFinalWestn.Models;
using AvaliacaoFinalWestn.Repository;
using Microsoft.AspNetCore.Authorization;

namespace AvaliacaoFinalWestn.Controllers;

public class AdministracaoController : Controller
{
    private readonly ICarroRepository _carroRepository;

    public AdministracaoController(ICarroRepository carroRepository)
    {
        _carroRepository = carroRepository;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult CadastroVeiculo()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditarCarro(int id)
    {
        var carro = await _carroRepository.BuscarPorIdAsync(id);
        EditarCarroViewModel vm = new EditarCarroViewModel()
        {
            Id = carro.Id,
            Nome = carro.Nome,
            Fabricante = carro.Fabricante,
            Motor = carro.Motor,
            Potencia = carro.Potencia,
            Torque = carro.Torque,
            ZeroaAcem = carro.ZeroaAcem,
            Quantidade = carro.Quantidade,
            VelocidadeMax = carro.VelocidadeMax,
            Transmissao = carro.Transmissao,
            Tracao = carro.Tracao,
            Preco = carro.Preco
        };

        return View(vm);
    }

    public async Task<IActionResult> DeletarCarro(int id)
    {
        var carro = await _carroRepository.BuscarPorIdAsync(id);

        if (carro == null)
        {
            TempData["Erro"] = "Carro não encontrado.";
            return RedirectToAction("Colecao", "Carro");
        }

        await _carroRepository.Deletar(carro.Id);
        return RedirectToAction("Colecao", "Carro");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}