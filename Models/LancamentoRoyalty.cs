namespace Franqueada.API.Models;

public class LancamentoRoyalty
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public Unidade? Unidade { get; set; }

    public int MesReferencia { get; set; }
    public int AnoReferencia { get; set; }
    public decimal FaturamentoPeriodo { get; set; }
    public decimal PercentualAplicado { get; set; }
    public decimal ValorDevido => FaturamentoPeriodo * (PercentualAplicado / 100m);

    public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;
    public DateTime? DataPagamento { get; set; }
}