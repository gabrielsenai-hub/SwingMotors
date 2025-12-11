using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AvaliacaoFinalWestn.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using AvaliacaoFinalWestn.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AvaliacaoFinalWestn.Models.ViewModels;
using System.Globalization;

namespace AvaliacaoFinalWestn.Controllers;

public class CarroController : Controller
{
    private readonly ICarroRepository _carroRepository;
    private readonly UserManager<Usuario> _userManager;
    private readonly ICarroCompradoRepository _carroCompradoRepository;
    private readonly IEnderecoRepository _endereçoRepository;
    public CarroController(IEnderecoRepository enderecoRepository, ICarroRepository carroRepository, UserManager<Usuario> userManager, ICarroCompradoRepository carroCompradoRepository)
    {
        _endereçoRepository = enderecoRepository;
        _carroRepository = carroRepository;
        _userManager = userManager;
        _carroCompradoRepository = carroCompradoRepository;
    }

    public async Task<IActionResult> Foto(int id)
    {
        var img = _carroRepository.BuscarImgPorId(id);

        if (img == null) return NotFound();

        return File(img.Imagem, img.ContentType);
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar(RegistroCarroViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            foreach (var erro in ModelState)
            {
                Console.WriteLine($"{erro.Key}: {string.Join(", ", erro.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            TempData["ErroCarro"] = string.Join(" | ",
                ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            );
            return RedirectToAction("CadastroVeiculo", "Administracao");
        }
        if (vm.ArquivosImagens.Count > 5)
        {
            TempData["ErroCarro"] = "Um carro só pode ter no máximo 5 imagens!";
            return RedirectToAction("CadastroVeiculo", "Administracao");
        }
        var carro = new Carro
        {
            Nome = vm.Nome,
            Fabricante = vm.Fabricante,
            Motor = vm.Motor,
            Potencia = vm.Potencia,
            Torque = vm.Torque,
            ZeroaAcem = vm.ZeroaAcem,
            Quantidade = vm.Quantidade,
            VelocidadeMax = vm.VelocidadeMax,
            Transmissao = vm.Transmissao,
            Tracao = vm.Tracao,
            Preco = vm.Preco,
        };

        var imagens = new List<CarroImagem>();

        if (vm.ArquivosImagens != null && vm.ArquivosImagens.Any())
        {
            foreach (var file in vm.ArquivosImagens)
            {
                if (file.Length <= 0) continue;

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                var imagemBytes = ms.ToArray();

                imagens.Add(new CarroImagem(
                    carroId: 0, // o EF vai preencher depois
                    imagem: imagemBytes,
                    contentType: file.ContentType
                ));
            }
        }

        await _carroRepository.Criar(carro, imagens);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ActionName("EditarCarro")]
    public async Task<IActionResult> EditarCarro(
        EditarCarroViewModel vm,
        [FromForm] int[]? idsImagensParaDeletar)
    {
        if (!ModelState.IsValid)
        {
            if (vm.ImagensExistentes == null)
            {
                vm.ImagensExistentes = await _carroRepository.BuscarImagens(vm.Id);
            }
            return View(vm);
        }

        var carro = await _carroRepository.BuscarPorIdAsync(vm.Id);

        if (carro == null)
        {
            TempData["ErroCarro"] = "Veículo não encontrado para edição.";
            return RedirectToAction("Colecao", "Carro");
        }

        // 2. Lógica de Exclusão: Se houver IDs para deletar, chame o método de exclusão.
        if (idsImagensParaDeletar != null && idsImagensParaDeletar.Length > 0)
        {
            await _carroRepository.DeletarImagensAsync(idsImagensParaDeletar); // Você precisa criar este método!
        }

        // 3. Atualiza as propriedades do Carro com os dados da ViewModel
        carro.Nome = vm.Nome;
        carro.Fabricante = vm.Fabricante;
        carro.Motor = vm.Motor;
        carro.Potencia = vm.Potencia;
        carro.Torque = vm.Torque;
        carro.ZeroaAcem = vm.ZeroaAcem;
        carro.Quantidade = vm.Quantidade;
        carro.VelocidadeMax = vm.VelocidadeMax;
        carro.Transmissao = vm.Transmissao;
        carro.Tracao = vm.Tracao;
        carro.Preco = vm.Preco;

        // 4. Edita o carro e salva as Novas Imagens (a exclusão já foi feita acima)
        await _carroRepository.Editar(carro, vm.NovosArquivosImagens);

        TempData["SucessoCarro"] = "Veículo editado com sucesso!";
        return RedirectToAction("Colecao", "Carro");
    }
    public async Task<IActionResult> DetalhesVeiculo(int id)
    {
        var carro = await _carroRepository.BuscarPorIdAsync(id);

        if (carro == null)
            return NotFound();

        var vm = new CarroDetalheVm
        {
            Carro = carro,
            Fotos = carro.Fotos?.ToList() ?? new List<CarroImagem>()
        };

        return View(vm);
    }
    public async Task<IActionResult> DetalhesVeiculoComprado(int id)
    {
        // Busca pelo ID da COMPRA, não do carro
        var compra = await _carroCompradoRepository.BuscarCompraCompletaAsync(id);

        if (compra == null)
            return NotFound();
        var user = await _userManager.GetUserAsync(User);

        var endereco = _endereçoRepository.Buscar(user.Id);

        var vm = new CarroCompradoVm
        {
            CompraId = compra.Id,
            Nome = compra.Carro.Nome,
            Fabricante = compra.Carro.Fabricante,
            PrecoPago = compra.PrecoPago,
            DataCompra = compra.DataCompra,
            Endereco = endereco,
            Fotos = compra.Carro.Fotos
                .Select(f => Url.Action("Foto", "Carro", new { id = f.Id }))
                .ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> FinalizarCompra(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            TempData["AlertaCadastro"] = "É necessário ter cadastro para realizar uma compra!";
            return RedirectToAction("Cadastro", "Home");
        }

        var carro = await _carroRepository.BuscarPorIdAsync(id);

        if (carro.Quantidade == 0)
            TempData["ErroCarro"] = "Desculpe, não há exemplares disponíveis!";


        var vm = new CarroDetalheVm
        {
            Carro = carro,
            Fotos = carro.Fotos?.ToList() ?? new List<CarroImagem>()
        };

        return View(vm);

    }

    [HttpPost("Finalizar")]
    public async Task<IActionResult> Finalizar(int id, string PrecoPago)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        try
        {

            await _carroRepository.ComprarCarro(id, user, PrecoPago);
            TempData["sucesso"] = "Carro comprado com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["erro"] = ex.Message;
        }

        return RedirectToAction("MeusVeiculos");
    }


    public async Task<IActionResult> Colecao()
    {
        var carros = await _carroRepository.Listar();

        var vm = carros.Select(c => new CarroVm
        {
            Id = c.Id,
            Nome = c.Nome,
            Preco = c.Preco.ToString("C"),
            Fabricante = c.Fabricante,
            Quantidade = c.Quantidade,
            Url = Url.Action("DetalhesVeiculo", "Carro", new { id = c.Id }),
            ImgId = c.Fotos != null && c.Fotos.Count > 0
                ? c.Fotos.First().Id
                : 0
        }).ToList();

        return View(vm);
    }

    public async Task<IActionResult> MeusVeiculos()
    {
        var user = await _userManager.GetUserAsync(User);

        var compras = await _carroRepository.Listar(user.Id);

        var vm = compras.Select(cc => new CarroVm
        {
            Id = cc.Carro.Id,
            Nome = cc.Carro.Nome,
            Preco = cc.Carro.Preco.ToString("C", new System.Globalization.CultureInfo("pt-BR")),
            Fabricante = cc.Carro.Fabricante,
            ImgId = cc.Carro.Fotos?.FirstOrDefault()?.Id ?? 0,
            Url = Url.Action("DetalhesVeiculoComprado", "Carro", new { id = cc.Id })

        })
        .ToList();

        return View(vm);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}