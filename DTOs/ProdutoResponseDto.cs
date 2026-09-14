using Franqueada.API.Models;
namespace Franqueada.API.DTOs;

public sealed class ProdutoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoBase { get; set; }
    public string? Categoria { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativado;

    public int FornecedorID {get; set;}
    public int QuantidadeEstoque {get; set;}
}