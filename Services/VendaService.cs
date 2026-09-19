using Microsoft.EntityFrameworkCore;
using Franqueada.API.DTOs;
using Franqueada.API.Data;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public sealed class VendaService : IVendaService
{
private readonly AppDbContext _context;

public VendaService(AppDbContext context)
{
    _context = context;
}

public async Task<VendaResponseDto> RegistrarVendaAsync(CriarVendaRequestDto dto, CancellationToken cancellationToken = default)
{
    if (dto.Itens == null || !dto.Itens.Any())
        throw new ArgumentException("A venda deve conter pelo menos um item.");

    using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    try
    {
        var venda = new Venda
        {
            UnidadeId = dto.UnidadeId,
            DataVenda = DateTime.UtcNow,
            ValorTotal = 0
        };

        decimal totalCalculado = 0;

        foreach (var itemDto in dto.Itens)
        {
            var produto = await _context.Produtos.FindAsync(new object[] { itemDto.ProdutoId }, cancellationToken);
            if (produto == null)
                throw new KeyNotFoundException($"Produto ID {itemDto.ProdutoId} não foi encontrado.");

            // Checa e debita o estoque
            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(e => e.ProdutoId == itemDto.ProdutoId && e.UnidadeId == dto.UnidadeId, cancellationToken);

            if (estoque == null)
                {
                    throw new InvalidOperationException($"Estoque insuficiente para o produto '{produto.NomeProduto}'. Saldo disponível: {estoque?.Quantidade ?? 0}");   
                }
            
            if (estoque.Quantidade < itemDto.Quantidade)
                {
                 throw new InvalidOperationException(
                $"Estoque insuficiente para '{produto.NomeProduto}'. Saldo atual: {estoque.Quantidade}, Solicitado: {itemDto.Quantidade}."
                );   
                }

            estoque.Quantidade -= itemDto.Quantidade;

            var itemVenda = new ItemVenda
            {
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.Preco
            };

            totalCalculado += itemVenda.Subtotal;
            venda.Itens.Add(itemVenda);

            // Adiciona registro na movimentação de estoque
            _context.MovimentacoesEstoque.Add(new MovimentoEstoque
            {
                Estoque = estoque,
                Quantidade = itemDto.Quantidade,
                Tipo = TipoMovimentacao.Saida,
                Observacao = $"Saída via Venda"
            });
        }

        venda.ValorTotal = totalCalculado;

        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await ObterPorIdAsync(venda.Id, cancellationToken) 
            ?? throw new Exception("Erro ao recuperar a venda criada.");
    }
    catch
    {
        await transaction.RollbackAsync(cancellationToken);
        throw;
    }
}

public async Task<VendaResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
{
    var venda = await _context.Vendas
        .Include(v => v.Itens)
        .ThenInclude(i => i.Produto)
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    if (venda == null) return null;

    return new VendaResponseDto
    {
        Id = venda.Id,
        UnidadeId = venda.UnidadeId,
        DataVenda = venda.DataVenda,
        ValorTotal = venda.ValorTotal,
        Itens = venda.Itens.Select(i => new ItemVendaResponseDto
        {
            ProdutoId = i.ProdutoId,
            NomeProduto = i.Produto?.NomeProduto ?? string.Empty,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.Subtotal
        }).ToList()
    };
}
}