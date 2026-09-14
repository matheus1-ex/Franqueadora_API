namespace Franqueada.API.Models;
public class Fornecedor
{
    public int Id { get; set; }
    public string NomeRazaoSocial { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativado;

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
} 