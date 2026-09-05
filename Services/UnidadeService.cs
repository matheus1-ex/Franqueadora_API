using Microsoft.EntityFrameWorkCore;
using Franqueada.API.Models;
using Franqueada.API.Data;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;

public class UnidadeService : IUnidadeService
    {
        private readonly AppDbContext _contexto;

        public UnidadeService(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        // Listagem com filtros de Nome e Status
        public async Task<IReadOnlyCollection<UnidadeResponseDto>> ObterTodasAsync(
            StatusAtivo? status, 
            string? nome, 
            CancellationToken cancellationToken = default)
        {
            var query = _contexto.Unidades.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(u => u.NomeUnidade.ToLower().Contains(nome.ToLower()));
            }

            if (status.HasValue)
            {
                bool statusBool = status.Value == StatusAtivo.Ativado;
                query = query.Where(unidade => unidade.StatusAtivo = status.Value == statusBool);
            }

            var unidades = await query.ToListAsync(cancellationToken);

            return unidades.Select(unidade => MapToResponseDto(unidade)).ToList().AsReadOnly();
        }

        // Busca por ID
        public async Task<UnidadeResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var unidade = await _contexto.Unidades
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (unidade == null) return null;

            return MapToResponseDto(unidade);
        }

        // Criação da Unidade + Geração automática do Código Identificador
        public async Task<UnidadeResponseDto> CriarAsync(UnidadeRequestDto dto, CancellationToken cancellationToken = default)
        {
            var novaUnidade = new Unidade
            {
                Nome_Unidade = dto.Nome_Unidade,
                Endereco = dto.Endereco,
                FranquiaId = dto.FranquiaId,
                StatusAtivos = true, // Toda unidade nasce ativa
                
                // Regra de Negócio: Geração do Código Único
                Cod_Identificador = $"UNI-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };

            _contexto.Unidades.Add(novaUnidade);
            await _contexto.SaveChangesAsync(cancellationToken);

            return MapToResponseDto(novaUnidade);
        }

        // Alternar Status (Ativado / Desativado)
        public async Task<bool> AlternarStatusAtivoAsync(int id, CancellationToken cancellationToken = default)
        {
            var unidade = await _contexto.Unidades.FindAsync(new object[] { id }, cancellationToken);

            if (unidade == null) return false;

            unidade.StatusAtivo = !unidade.StatusAtivo; // Inverte o status atual

            await _contexto.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Método auxiliar de mapeamento
        private static UnidadeResponseDto MapToResponseDto(Unidade unidade)
        {
            return new UnidadeResponseDto
            {
                id = unidade.Id,
                nome = unidade.Nome_Unidade,
                endereco = unidade.Endereco,
                cod_identificador = unidade.Cod_Identificador,
                status = unidade.StatusAtivos,
                franquiaId = unidade.FranquiaId
            };
        }
    }