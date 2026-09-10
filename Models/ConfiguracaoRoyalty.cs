namespace Franqueada.API.Models;
public sealed class ConfiguracaoRoyalty
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public Unidade? Unidade { get; set; }
    public decimal PercentualRoyalty { get; set; } // Ex: 5.00 para 5%
}