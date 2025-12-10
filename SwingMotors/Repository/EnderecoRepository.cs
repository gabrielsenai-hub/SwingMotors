using AvaliacaoFinalWestn.Data;
using AvaliacaoFinalWestn.Models;

namespace AvaliacaoFinalWestn.Repository;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly Context _context;

    public EnderecoRepository(Context context)
    {
        _context = context;
    }

    public void Criar(Endereco endereco)
    {
        _context.Enderecos.Add(endereco);
        _context.SaveChanges();
    }

    public void Editar(int id, Endereco endereco)
    {
        var ender = _context.Enderecos.FirstOrDefault(n => n.Id == id);
        if (!(ender == null))
        {
            _context.Enderecos.Update(endereco);
            _context.SaveChanges();
        }
    }

    public void Deletar(int id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(n => n.Id == id);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            _context.SaveChanges();
        }
    }

    public Endereco Buscar(int id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(n => n.Id == id);
        if (endereco != null)
        {
            return endereco;
        }

        return null;
    }
    public Endereco Buscar(string id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(n => n.UsuarioId == id);
        if (endereco != null)
        {
            return endereco;
        }

        return null;
    }
}