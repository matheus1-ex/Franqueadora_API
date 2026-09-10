namespace Franqueada.API.DTOs;

public class ItemVendaRequestDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}

public class CriarVendaRequestDto
{
    public int UnidadeId { get; set; }
    public List<ItemVendaRequestDto> Itens { get; set; } = new();
}

public class ItemVendaResponseDto
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class VendaResponseDto
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemVendaResponseDto> Itens { get; set; } = new();
}