using System.ComponentModel.DataAnnotations.Schema;

namespace AvaliacaoFinalWestn.Models;

public class Endereco
{
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; }

    public string Cep { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}