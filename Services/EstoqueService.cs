using Microsoft.EntityFrameworkCore;
using Franqueada.API.Data;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public sealed class EstoqueService : IEstoqueService
{
    private readonly AppDbContext _context;

    public EstoqueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> ObterSaldoAsync(int produtoId, int unidadeId, CancellationToken cancellationToken = default)
    {
        var estoque = await _context.Estoques
            .FirstOrDefaultAsync(e => e.ProdutoId == produtoId && e.UnidadeId == unidadeId, cancellationToken);

        return estoque?.Quantidade ?? 0;
    }

    public async Task<List<Estoque>> ObterItensAbaixoDoMinimoAsync(int unidadeId, int limiteMinimo, CancellationToken cancellationToken = default)
    {
        return await _context.Estoques
            .Include(e => e.Produto)
            .Where(e => e.UnidadeId == unidadeId && e.Quantidade <= limiteMinimo)
            .ToListAsync(cancellationToken);
    }

    public async Task MovimentarEstoqueAsync(int produtoId, int unidadeId, int quantidade, TipoMovimentacao tipo, string observacao, CancellationToken cancellationToken = default)
    {
        var estoque = await _context.Estoques
            .FirstOrDefaultAsync(e => e.ProdutoId == produtoId && e.UnidadeId == unidadeId, cancellationToken);

        if (estoque == null)
        {
            estoque = new Estoque
            {
                ProdutoId = produtoId,
                UnidadeId = unidadeId,
                Quantidade = 0
            };
            _context.Estoques.Add(estoque);
        }

        if (tipo == TipoMovimentacao.Entrada)
        {
            estoque.Quantidade += quantidade;
        }
        else if (tipo == TipoMovimentacao.Saida)
        {
            if (estoque.Quantidade < quantidade)
                throw new InvalidOperationException($"Estoque insuficiente. Saldo disponível: {estoque.Quantidade}");

            estoque.Quantidade -= quantidade;
        }

        _context.MovimentacoesEstoque.Add(new MovimentoEstoque
        {
            Estoque = estoque,
            Quantidade = quantidade,
            Tipo = tipo,
            Observacao = observacao
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

}