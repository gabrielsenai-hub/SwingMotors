using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoFinalWestn.Models;
using AvaliacaoFinalWestn.Models.ViewModels;
using AvaliacaoFinalWestn.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AvaliacaoFinalWestn.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IConfiguration _config;

        public UsuarioController(IConfiguration configuration, IEnderecoRepository enderecoRepository, ILogger<UsuarioController> logger,
            UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _config = configuration;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _enderecoRepository = enderecoRepository;
        }

        [HttpPost("Cadastro")]
        public async Task<IActionResult> Cadastro(RegistroViewModel model)
        {
            var endereco = new Endereco
            {
                Cep = model.Cep,
                Rua = model.Rua,
                Numero = model.Numero,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
            };
            var user = new Usuario()
            {
                UserName = model.Email,
                Email = model.Email,
                Nome = model.Nome,
                Telefone = model.Telefone,
                Cpf = model.Cpf,
                Endereco = endereco
            };

            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {
                // 1. Checa se nome tem o código especial
                if (model.Nome.Contains(_config["AdminKey"]))
                {
                    // 2. Adiciona na role Admin
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                // 3. Faz login automático
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            // ERROS
            foreach (var error in result.Errors)
                TempData["ErroCadastro"] += error.Description + "\n";

            return RedirectToAction("Cadastro", "Home");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, // ← Aqui estamos usando o email como UserName
                model.Senha,
                model.LembrarMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            TempData["ErroCadastro"] = "Credenciais inválidas!";
            return RedirectToAction("Login", "Home");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(EditarUsuarioViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null)
                throw new Exception("Usuario não encontrado!");

            // Verifica se o email será alterado
            if (User.Identity.Name != model.Email)
            {
                var same = await _userManager.FindByEmailAsync(model.Email);
                if (same != null)
                {
                    TempData["ConfigAlert"] = "Já existe um usuário com este endereço de email!";
                    return RedirectToAction("Configuracoes");
                }

                await _userManager.SetEmailAsync(user, model.Email);
            }

            // Atualiza endereço
            var endereco = _enderecoRepository.Buscar(user.Id) ?? new Endereco { UsuarioId = user.Id };

            endereco.Numero = model.Numero;
            endereco.Cep = model.Cep;
            endereco.Bairro = model.Bairro;
            endereco.Rua = model.Rua;
            endereco.Cidade = model.Cidade;
            endereco.Estado = model.Estado;

            _enderecoRepository.Editar(endereco.Id, endereco);
            user.Endereco = endereco;

            // Atualiza dados principais
            user.PhoneNumber = model.Telefone;
            user.Cpf = model.Cpf;
            user.Nome = model.Nome;

            var adminKey = _config["AdminKey"]; // mais seguro do que colocar no nome

            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // Usuário quer ser admin
            if (model.Nome.Contains(adminKey) && !isAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            // Usuário deve perder admin
            else if (!model.Nome.Contains(adminKey) && isAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            await _userManager.UpdateAsync(user);

            // Se for alterar senha
            if (!string.IsNullOrEmpty(model.SenhaNova))
            {
                bool senhaCorreta = await _userManager.CheckPasswordAsync(user, model.SenhaAtual);
                if (!senhaCorreta)
                {
                    TempData["ConfigAlert"] = "Senha atual incorreta!";
                    return RedirectToAction("Configuracoes");
                }

                await _userManager.ChangePasswordAsync(user, model.SenhaAtual, model.SenhaNova);
            }

            TempData["ConfigSuccess"] = "Alterações concluídas com sucesso!";
            return RedirectToAction("Configuracoes", "Usuario");
        }

        [HttpPost("Excluir")]
        public async Task<IActionResult> Excluir()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                await _signInManager.SignOutAsync();

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Configuracoes")]
        public async Task<IActionResult> Configuracoes()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                throw new Exception("Usuario nulo");
            }

            var endereco = _enderecoRepository.Buscar(user.Id); // pode ser nulo

            var vm = new EditarUsuarioViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Nome = user.Nome,
                Telefone = user.Telefone,
                Cpf = user.Cpf,
                Numero = endereco?.Numero,
                Cep = endereco?.Cep,
                Bairro = endereco?.Bairro,
                Rua = endereco?.Rua,
                Cidade = endereco?.Cidade,
                Estado = endereco?.Estado
            };

            return View(vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}