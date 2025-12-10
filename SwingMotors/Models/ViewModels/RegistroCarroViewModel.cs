using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AvaliacaoFinalWestn.Models;

public class RegistroCarroViewModel
{
    public string Nome { get; set; }
    public string Fabricante { get; set; }
    public string Motor { get; set; }
    public string Potencia { get; set; }
    public string Torque { get; set; }
    public string ZeroaAcem { get; set; }
    public int Quantidade { get; set; }
    public string VelocidadeMax { get; set; }
    public string Transmissao { get; set; }
    public string Tracao { get; set; }
    public double Preco { get; set; }
    [Display(Name = "Fotos")]
    public List<IFormFile> ArquivosImagens { get; set; }
    
}