using Microsoft.EntityFrameworkCore;
using Franqueada.API.Data;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public class ChamadoService : IChamadoService
    {
        private readonly AppDbContext _contexto;

        public ChamadoService(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<ChamadoResponseDto> CriarAsync(CriarChamadoDto dto, CancellationToken cancellationToken)
        {
            var chamado = new Chamado
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade,
                Status = StatusChamado.Aberto,
                DataAbertura = DateTime.UtcNow
            };

            _contexto.Chamados.Add(chamado);
            await _contexto.SaveChangesAsync(cancellationToken);

            return await ObterPorIdAsync(chamado.Id, cancellationToken) 
                ?? throw new InvalidOperationException("Erro ao carregar chamado criado.");
        }

        public async Task<ChamadoResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _contexto.Chamados
                .AsNoTracking()
                .Include(c => c.Unidade)
                .Where(c => c.Id == id)
                .Select(c => new ChamadoResponseDto
                {
                    Id = c.Id,
                    UnidadeId = c.UnidadeId,
                    NomeUnidade = c.Unidade != null ? c.Unidade.Nome : "N/A",
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade,
                    Status = c.Status,
                    DataAbertura = c.DataAbertura,
                    DataAtualizacao = c.DataAtualizacao,
                    DataEncerramento = c.DataEncerramento,
                    ObservacoesEncerramento = c.ObservacoesEncerramento
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ChamadoResponseDto>> ObterTodosAsync(
            int? unidadeId, 
            StatusChamado? status, 
            CancellationToken cancellationToken)
        {
            var query = _contexto.Chamados
                .AsNoTracking()
                .Include(c => c.Unidade)
                .AsQueryable();

            if (unidadeId.HasValue)
                query = query.Where(c => c.UnidadeId == unidadeId.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return await query
                .OrderByDescending(c => c.DataAbertura)
                .Select(c => new ChamadoResponseDto
                {
                    Id = c.Id,
                    UnidadeId = c.UnidadeId,
                    NomeUnidade = c.Unidade != null ? c.Unidade.Nome : "N/A",
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade,
                    Status = c.Status,
                    DataAbertura = c.DataAbertura,
                    DataAtualizacao = c.DataAtualizacao,
                    DataEncerramento = c.DataEncerramento,
                    ObservacoesEncerramento = c.ObservacoesEncerramento
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AtualizarStatusAsync(int id, AtualizarStatusChamadoDto dto, CancellationToken cancellationToken)
        {
            var chamado = await _contexto.Chamados.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (chamado == null) return false;

            chamado.Status = dto.NovoStatus;
            chamado.DataAtualizacao = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(dto.Observacao))
                chamado.ObservacoesEncerramento = dto.Observacao;

            if (dto.NovoStatus == StatusChamado.Concluido || dto.NovoStatus == StatusChamado.Cancelado)
            {
                chamado.DataEncerramento = DateTime.UtcNow;
            }

            await _contexto.SaveChangesAsync(cancellationToken);
            return true;
        }
    }