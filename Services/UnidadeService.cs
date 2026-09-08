using Microsoft.EntityFrameworkCore;
using Franqueada.API.Models;
using Franqueada.API.Data;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;

public class UnidadeService : IUnidadeService
    {
        private readonly AppContext _contexto;

        public UnidadeService(AppContext contexto)
        {
            _contexto = contexto;
        }

        // Listagem com filtros de Nome e Status
        public async Task<IReadOnlyCollection<UnidadeResponseDto>> ObterTodasAsync(
            StatusAtivo? status,
            string? busca,
            string? nome, 
            CancellationToken cancellationToken)
        {
            var query = _contexto.Unidades.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(unidade => unidade.Nome_Unidade.ToLower().Contains(nome.ToLower()));
            }

            if (status.HasValue)
            {
                query = query.Where(unidade => unidade.StatusAtivo = status.Value);
            }

            // Busca pelo Nome ou Código Identificador
            if (!string.IsNullOrWhiteSpace(busca))
            string termo = busca.Trim();
            query = query.Where(
                unidade => unidade.Nome_Unidade.Contains(termo) || unidade.Cod_Identificador.Contains(busca)
            );

            // Execução do banco SQL
            List<Unidade> unidades = await query.OrderByDescending(unidade => unidade.Id_Und).ToListAsync(cancellationtoken);

            return unidades.Select(unidades => MapToResponseDto(unidade)).ToList().AsReadOnly();
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
                Status = statusAtivo.Ativado,
                
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

            unidade.StatusAtivo = !unidade.StatusAtivo;

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
                status = unidade.Status.ToString(),
                franquiaId = unidade.FranquiaId
            };
        }

    public Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(Nome_Unidade? nome_Unid, int id, StatusAtivo? status, Cod_Identificar? cod_Identificar, Endereco? endereco, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UnidadeResponsetDto> CriarAsync(UnidadeResquestDto criar, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarStatusAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}