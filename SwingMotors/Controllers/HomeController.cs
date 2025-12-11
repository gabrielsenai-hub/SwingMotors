using System.Diagnostics;
using AvaliacaoFinalWestn.Data;
using Microsoft.AspNetCore.Mvc;
using AvaliacaoFinalWestn.Models;
using AvaliacaoFinalWestn.Repository;
using Microsoft.AspNetCore.Identity;

namespace AvaliacaoFinalWestn.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<Usuario> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Contato()
    {
        if (!User.Identity.IsAuthenticated)
            return View();

        var user = await _userManager.GetUserAsync(User);

        ViewBag.Nome = user.Nome;
        ViewBag.Email = user.Email;

        return View();
    }

    public IActionResult Email()
    {
        TempData["ContatoSuccess"] = "Mensagem enviada com sucesso!";
        return RedirectToAction("Index");
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public IActionResult Cadastro()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}