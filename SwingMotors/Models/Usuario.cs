using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AvaliacaoFinalWestn.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public DateTime Registro_criacao { get; set; } = DateTime.UtcNow;
        public Endereco Endereco { get; set; }
        public List<CarroComprado>? CarroComprados { get; set; }
        
        
        
    }
}