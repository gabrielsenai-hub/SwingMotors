using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaliacaoFinalWestn.Models.ViewModels
{
    public class CarroCompradoVm
    {
        public int CompraId { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public string PrecoPago { get; set; }
        public DateTime DataCompra { get; set; }
        public List<string> Fotos { get; set; }
    }
}