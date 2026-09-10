using Franqueada.API.Services;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Franqueada.API.Data;

namespace Franqueada.API.Services;
public sealed class EstoqueService : IEstoqueService
    {
        private readonly AppDbContext _context;

        public EstoqueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstoqueResponseDto?> ObterSaldoAsync(int produtoId, int unidadeId, CancellationToken cancellationToken = default)
        {
            var estoque = await _context.Estoques.Include(e => e.Produto)
                .Include(estoque => estoque.Unidade)
                .AsNoTracking()
                .FirstOrDefaultAsync(estoque => estoque.ProdutoId == produtoId && estoque.UnidadeId == unidadeId, cancellationToken);

            if (estoque == null) return null;

            return MapToDto(estoque);
        }

        public async Task<IReadOnlyCollection<EstoqueResponseDto>> ObterItensAbaixoDoMinimoAsync(int? unidadeId, CancellationToken cancellationToken = default)
        {
            var query = _context.Estoques
                .Include(estoque => estoque.Produto)
                .Include(estoque => estoque.Unidade)
                .AsNoTracking()
                .Where(estoque => estoque.Quantidade < estoque.EstoqueMinimo);

            if (unidadeId.HasValue)
                query = query.Where(estoque => estoque.UnidadeId == unidadeId.Value);

            var itens = await query.ToListAsync(cancellationToken);
            return itens.Select(MapToDto).ToList().AsReadOnly();
        }

        public async Task<bool> MovimentarEstoqueAsync(MovimentarEstoqueDto dto, CancellationToken cancellationToken = default)
        {
            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(estoque => estoque.ProdutoId == dto.ProdutoId && estoque.UnidadeId == dto.UnidadeId, cancellationToken);

            if (estoque == null)
            {
                // Se não existir o registro de estoque para o produto na unidade, cria um novo
                if (dto.Tipo == TipoMovimentacao.Saida)
                    throw new InvalidOperationException("Não é possível realizar saída de um estoque inexistente.");

                estoque = new Estoque
                {
                    ProdutoId = dto.ProdutoId,
                    UnidadeId = dto.UnidadeId,
                    Quantidade = 0,
                    EstoqueMinimo = 5 // Valor padrão
                };
                _context.Estoques.Add(estoque);
            }

            // Regra: Impedimento de saldo negativo
            if (dto.Tipo == TipoMovimentacao.Saida && estoque.Quantidade < dto.Quantidade)
            {
                throw new InvalidOperationException("Saldo insuficiente em estoque para realizar a saída.");
            }

            // Atualiza o saldo
            if (dto.Tipo == TipoMovimentacao.Entrada)
                estoque.Quantidade += dto.Quantidade;
            else
                estoque.Quantidade -= dto.Quantidade;

            // Histórico
            var movimentacao = new MovimentoEstoque
            {
                Estoque = estoque,
                Quantidade = dto.Quantidade,
                Tipo = dto.Tipo,
                Observacao = dto.Observacao
            };

            _context.MovimentacoesEstoque.Add(movimentacao);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        private static EstoqueResponseDto MapToDto(Estoque estoque) => new()
        {
            Id = estoque.Id,
            Id_Produto = estoque.ProdutoId,
            Nome_Produto = estoque.Produto?.NomeProduto ?? string.Empty,
            UnidadeId = estoque.UnidadeId,
            Nome_Unidade = estoque.Unidade?.Nome ?? string.Empty,
            Quantidade = estoque.Quantidade,
            EstoqueMinimo = estoque.EstoqueMinimo
        };
    }