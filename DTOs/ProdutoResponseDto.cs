namespace Franqueada.API.DTOs;

public sealed class ProdutoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public double PrecoBase { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Status { get; set; }
}