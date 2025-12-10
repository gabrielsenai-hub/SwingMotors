using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaliacaoFinalWestn.Models
{
public class CarroComprado
{
    public int Id { get; set; }

    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public string PrecoPago { get; set; }
    public int CarroId { get; set; }
    public Carro Carro { get; set; }
    public DateTime DataCompra { get; set; } = DateTime.UtcNow;
}
}