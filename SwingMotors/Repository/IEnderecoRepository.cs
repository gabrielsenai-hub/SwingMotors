using AvaliacaoFinalWestn.Models;

namespace AvaliacaoFinalWestn.Repository;

public interface IEnderecoRepository
{
    public void Criar(Endereco endereco);
    public void Editar(int id, Endereco endereco);
    public void Deletar(int id);
    public Endereco Buscar(int id);
    public Endereco Buscar(string id);
}