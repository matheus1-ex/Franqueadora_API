using Franqueada.API.Models;
namespace Franqueada.API.DTOs;

// 1. Faturamento por Unidade e Período
public sealed class FaturamentoUnidadeDto
{
    public int UnidadeId { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public decimal TotalFaturamento { get; set; }
    public int TotalVendas { get; set; }
}

// 2. Ranking de Unidades
public sealed class RankingUnidadeDto
{
    public int Posicao { get; set; }
    public int UnidadeId { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public decimal TotalFaturado { get; set; }
}

// 3. Total de Royalties
public sealed class ResumoRoyaltiesDto
{
    public decimal TotalGerado { get; set; }
    public decimal TotalPago { get; set; }
    public decimal TotalPendente { get; set; }
    public int QuantidadeLancamentos { get; set; }
}

// 4. Produtos mais vendidos
public sealed class ProdutoMaisVendidoDto
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int QuantidadeVendida { get; set; }
    public decimal TotalArrecadado { get; set; }
}

// 5. Estoque Crítico
public sealed class EstoqueCriticoDto
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public string StatusEstoque => QuantidadeAtual == 0 ? "Esgotado" : "Abaixo do Mínimo";
}

// 6. Chamados por Status
public sealed class ChamadosPorStatusDto
{
    public StatusChamado Status { get; set; }
    public string StatusNome => Status.ToString();
    public int Quantidade { get; set; }
}