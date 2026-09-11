using Franqueada.API.Models;

namespace Franqueada.API.DTOs;

public sealed class SalvarConfiguracaoRoyaltyDto
{
    public int UnidadeId { get; set; }
    public decimal PercentualRoyalty { get; set; }
}

public sealed class GerarRoyaltyDto
{
    public int UnidadeId { get; set; }
    public int MesReferencia { get; set; }
    public int AnoReferencia { get; set; }
}

public sealed class RoyaltyResponseDto
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public int MesReferencia { get; set; }
    public int AnoReferencia { get; set; }
    public decimal FaturamentoPeriodo { get; set; }
    public decimal PercentualAplicado { get; set; }
    public decimal ValorDevido { get; set; }
    public StatusPagamento Status { get; set; }
    public DateTime? DataPagamento { get; set; }
}