namespace AvaliacaoFinalWestn.Models;

public class RegistroViewModel
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Senha { get; set; }
    public string Cep { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }

    public override string ToString()
    {
        return $"Nome: {Nome}\n" +
               $"CPF: {Cpf}\n" +
               $"Email: {Email}\n" +
               $"Telefone: {Telefone}\n" +
               $"Senha: {Senha}\n" +
               $"CEP: {Cep}\n" +
               $"Rua: {Rua}\n" +
               $"Número: {Numero}\n" +
               $"Bairro: {Bairro}\n" +
               $"Cidade: {Cidade}\n" +
               $"Estado: {Estado}";
    }}