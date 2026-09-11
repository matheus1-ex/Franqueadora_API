namespace Franqueada.API.DTOs;

public sealed class ItemVendaRequestDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}

public sealed class CriarVendaRequestDto
{
    public int UnidadeId { get; set; }
    public List<ItemVendaRequestDto> Itens { get; set; } = new();
}

public sealed class ItemVendaResponseDto
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public sealed class VendaResponseDto
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemVendaResponseDto> Itens { get; set; } = new();
}