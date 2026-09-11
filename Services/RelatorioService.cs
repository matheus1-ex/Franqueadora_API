using Microsoft.EntityFrameworkCore;
using Franqueada.API.Data;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;
public class RelatorioService : IRelatorioService
{
    private readonly AppDbContext _context;

    public RelatorioService(AppDbContext context)
    {
        _context = context;
    }

    // 1. Faturamento por Unidade e Período
    public async Task<IEnumerable<FaturamentoUnidadeDto>> ObterFaturamentoPorUnidadeAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        return await _context.Vendas
            .AsNoTracking()
            .Where(v => v.DataVenda >= inicio && v.DataVenda <= fim)
            .GroupBy(v => new { v.UnidadeId, v.Unidade!.Nome })
            .Select(g => new FaturamentoUnidadeDto
            {
                UnidadeId = g.Key.UnidadeId,
                NomeUnidade = g.Key.Nome,
                TotalFaturamento = g.Sum(v => v.ValorTotal),
                TotalVendas = g.Count()
            })
            .ToListAsync(cancellationToken);
    }

    // 2. Ranking de Unidades por Faturamento
    public async Task<IEnumerable<RankingUnidadeDto>> ObterRankingUnidadesAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        var faturamento = await _context.Vendas
            .AsNoTracking()
            .Where(v => v.DataVenda >= inicio && v.DataVenda <= fim)
            .GroupBy(v => new { v.UnidadeId, v.Unidade!.Nome })
            .Select(g => new { g.Key.UnidadeId, g.Key.Nome, Total = g.Sum(v => v.ValorTotal) })
            .OrderByDescending(x => x.Total)
            .ToListAsync(cancellationToken);

        int posicao = 1;
        return faturamento.Select(x => new RankingUnidadeDto
        {
            Posicao = posicao++,
            UnidadeId = x.UnidadeId,
            NomeUnidade = x.Nome,
            TotalFaturado = x.Total
        });
    }

    // 3. Total de Royalties Gerados
    public async Task<ResumoRoyaltiesDto> ObterTotalRoyaltiesAsync(int? mes, int? ano, CancellationToken cancellationToken)
    {
        IQueryable<LancamentoRoyalty> query = _context.LancamentosRoyalty.AsNoTracking();

        if (mes.HasValue) query = query.Where(r => r.MesReferencia == mes.Value);
        if (ano.HasValue) query = query.Where(r => r.AnoReferencia == ano.Value);

        var royalties = await query.ToListAsync(cancellationToken);

        return new ResumoRoyaltiesDto
        {
            TotalGerado = royalties.Sum(r => r.ValorCalculado),
            TotalPago = royalties.Where(r => r.Pago).Sum(r => r.ValorCalculado),
            TotalPendente = royalties.Where(r => !r.Pago).Sum(r => r.ValorCalculado),
            QuantidadeLancamentos = royalties.Count
        };
    }

    // 4. Produtos Mais Vendidos
    public async Task<IEnumerable<ProdutoMaisVendidoDto>> ObterProdutosMaisVendidosAsync(int top, CancellationToken cancellationToken)
    {
        return await _context.ItensVenda
            .AsNoTracking()
            .GroupBy(i => new { i.ProdutoId, i.Produto!.Nome })
            .Select(g => new ProdutoMaisVendidoDto
            {
                ProdutoId = g.Key.ProdutoId,
                NomeProduto = g.Key.Nome,
                QuantidadeVendida = g.Sum(i => i.Quantidade),
                TotalArrecadado = g.Sum(i => i.Subtotal)
            })
            .OrderByDescending(x => x.QuantidadeVendida)
            .Take(top)
            .ToListAsync(cancellationToken);
    }

    // 5. Estoque Crítico
    public async Task<IEnumerable<EstoqueCriticoDto>> ObterEstoqueCriticoAsync(CancellationToken cancellationToken)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.QuantidadeEstoque <= p.QuantidadeMinima)
            .Select(p => new EstoqueCriticoDto
            {
                ProdutoId = p.Id,
                NomeProduto = p.Nome,
                QuantidadeAtual = p.QuantidadeEstoque,
                QuantidadeMinima = p.QuantidadeMinima
            })
            .ToListAsync(cancellationToken);
    }

    // 6. Quantidade de Chamados por Status
    public async Task<IEnumerable<ChamadosPorStatusDto>> ObterQuantidadeChamadosPorStatusAsync(CancellationToken cancellationToken)
    {
        return await _context.Chamados
            .AsNoTracking()
            .GroupBy(c => c.Status)
            .Select(g => new ChamadosPorStatusDto
            {
                Status = g.Key,
                Quantidade = g.Count()
            })
            .ToListAsync(cancellationToken);
    }
}