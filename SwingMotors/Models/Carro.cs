using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaliacaoFinalWestn.Models
{
    public class Carro
    {
        public Carro()
        {
            Registro_criacao = DateTime.UtcNow;
            Fotos = new List<CarroImagem>();

        }
        
                        public int Id { get; set; }
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
        public List<CarroImagem> Fotos { get; set; }
        public DateTime Registro_criacao { get; set; }



    }
}