using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Franqueada.API.Data;

namespace Franqueada.API.Services;

public class FinanceiroService : IFinanceiroService
    {
        private readonly AppDbContext _context;

        public FinanceiroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DefinirPercentualRoyaltyAsync(SalvarConfiguracaoRoyaltyDto dto, CancellationToken cancellationToken = default)
        {
            var config = await _context.ConfiguracoesRoyalty
                .FirstOrDefaultAsync(c => c.UnidadeId == dto.UnidadeId, cancellationToken);

            if (config == null)
            {
                config = new ConfiguracaoRoyalty
                {
                    UnidadeId = dto.UnidadeId,
                    PercentualRoyalty = dto.PercentualRoyalty
                };
                _context.ConfiguracoesRoyalty.Add(config);
            }
            else
            {
                config.PercentualRoyalty = dto.PercentualRoyalty;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RoyaltyResponseDto> CalcularEGerarRoyaltyAsync(GerarRoyaltyDto dto, CancellationToken cancellationToken = default)
        {
            var unidade = await _context.Unidades
            .Include(u => u.Franquia)
        .FirstOrDefaultAsync(u => u.Id_Und == dto.UnidadeId, cancellationToken);


        if (unidade == null)
        {
            throw new KeyNotFoundException("Unidade não encontrada.");   
        }

            var config = await _context.ConfiguracoesRoyalty
                .FirstOrDefaultAsync(c => c.UnidadeId == dto.UnidadeId, cancellationToken);

            if (config == null)
                throw new InvalidOperationException("Percentual de royalty não configurado para esta unidade.");

            decimal percentualRoyalty = config?.PercentualRoyalty 
            ?? unidade.Franquia?.PercentualRoyalty 
            ?? 5.0m;

        // Calcula o faturamento somando as vendas do mês/ano
        var faturamento = await _context.Vendas
            .Where(v => v.UnidadeId == dto.UnidadeId &&
                        v.DataVenda.Month == dto.MesReferencia &&
                        v.DataVenda.Year == dto.AnoReferencia)
            .SumAsync(v => v.ValorTotal, cancellationToken);

        var lancamento = new LancamentoRoyalty
        {
            UnidadeId = dto.UnidadeId,
            MesReferencia = dto.MesReferencia,
            AnoReferencia = dto.AnoReferencia,
            FaturamentoPeriodo = faturamento,
            PercentualAplicado = percentualRoyalty, // Usa a taxa com fallback
            Status = StatusPagamento.Pendente
        };

        _context.LancamentosRoyalty.Add(lancamento);
        await _context.SaveChangesAsync(cancellationToken);

        return await ObterPorIdAsync(lancamento.Id, cancellationToken);
        }

        public async Task<bool> RegistrarPagamentoAsync(int lancamentoId, CancellationToken cancellationToken = default)
        {
            var lancamento = await _context.LancamentosRoyalty.FindAsync(new object[] { lancamentoId }, cancellationToken);
            if (lancamento == null) return false;

            lancamento.Status = StatusPagamento.Pago;
            lancamento.DataPagamento = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IReadOnlyCollection<RoyaltyResponseDto>> ConsultarValoresPorUnidadeAsync(int? unidadeId, StatusPagamento? status, CancellationToken cancellationToken = default)
        {
            var query = _context.LancamentosRoyalty
                .Include(l => l.Unidade)
                .AsNoTracking();

            if (unidadeId.HasValue)
                query = query.Where(l => l.UnidadeId == unidadeId.Value);

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            var lista = await query.ToListAsync(cancellationToken);

            return lista.Select(l => new RoyaltyResponseDto
            {
                Id = l.Id,
                UnidadeId = l.UnidadeId,
                NomeUnidade = l.Unidade?.Nome ?? string.Empty,
                MesReferencia = l.MesReferencia,
                AnoReferencia = l.AnoReferencia,
                FaturamentoPeriodo = l.FaturamentoPeriodo,
                PercentualAplicado = l.PercentualAplicado,
                ValorDevido = l.ValorDevido,
                Status = l.Status,
                DataPagamento = l.DataPagamento
            }).ToList().AsReadOnly();
        }

        private async Task<RoyaltyResponseDto> ObterPorIdAsync(int id, CancellationToken ct)
        {
            var l = await _context.LancamentosRoyalty
                .Include(x => x.Unidade)
                .FirstAsync(x => x.Id == id, ct);

            return new RoyaltyResponseDto
            {
                Id = l.Id,
                UnidadeId = l.UnidadeId,
                NomeUnidade = l.Unidade?.Nome ?? string.Empty,
                MesReferencia = l.MesReferencia,
                AnoReferencia = l.AnoReferencia,
                FaturamentoPeriodo = l.FaturamentoPeriodo,
                PercentualAplicado = l.PercentualAplicado,
                ValorDevido = l.ValorDevido,
                Status = l.Status,
                DataPagamento = l.DataPagamento
            };
        }
    }