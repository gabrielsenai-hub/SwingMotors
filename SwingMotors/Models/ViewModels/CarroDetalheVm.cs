namespace AvaliacaoFinalWestn.Models;

public class CarroDetalheVm
{
    public Carro Carro { get; set; }
    public List<CarroImagem> Fotos { get; set; } = new();
    public string PrecoPago { get; set; }
}