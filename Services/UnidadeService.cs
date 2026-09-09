using Microsoft.EntityFrameworkCore;
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

        // Método auxiliar de mapeamento
        private static UnidadeResponseDto MapToResponseDto(Unidade unidade)
        {
            return new UnidadeResponseDto
            {
                Id_Und = unidade.Id_Und,
                Nome_Unidade = unidade.Nome,
                Endereco = unidade.Endereco,
                Cod_Identificador = unidade.Cod_Identificador,
                status = unidade.Status
            };
        }

        // Listagem com filtros de Nome e Status
        public async Task<IReadOnlyCollection<UnidadeResponseDto>> ObterTodasAsync(
            StatusAtivo? status,
            string? busca,
            string? nome, 
            CancellationToken cancellationToken)
        {
            IQueryable<Unidade> query = _contexto.Unidades.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(unidade => unidade.Nome.Contains(nome.ToLower()));
            }

            if (status.HasValue)
            {
                query = query.Where(unidade => unidade.Status == status);
            }

            // Busca pelo Nome ou Código Identificador
            if (!string.IsNullOrWhiteSpace(busca))
        {
            string termo = busca.Trim();
            query = query.Where(
                unidade => unidade.Nome != null || unidade.Cod_Identificador != null
            );   
        }

            // Execução do banco SQL
            List<Unidade> unidades = await query.OrderByDescending(unidade => unidade.Id_Und).ToListAsync(cancellationToken);

            return unidades.Select(unidades => MapToResponseDto(unidades)).ToList().AsReadOnly();
        }

        // Busca por ID
        public async Task<UnidadeResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var unidade = await _contexto.Unidades
                .AsNoTracking()
                .FirstOrDefaultAsync(und => und.Id_Und == id, cancellationToken);

            if (unidade == null) return null;

            return MapToResponseDto(unidade);
        }

        // Criação da Unidade + Geração automática do Código Identificador
        public async Task<UnidadeResponseDto> CriarAsync(UnidadeRequestDto dto, CancellationToken cancellationToken = default)
        {
            var novaUnidade = new Unidade
            {
                Nome = dto.NomeUnidade,
                Endereco = dto.Endereco,
                FranquiaID = dto.FranquiaId,
                Status = StatusAtivo.Ativado,
                
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

            unidade.Status = StatusAtivo.Ativado;

            await _contexto.SaveChangesAsync(cancellationToken);
            return true;
        }

    public Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(string? nome_Unid, int id, StatusAtivo? status, string? cod_Identificar, string? endereco, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UnidadeResponseDto> CriarAsync(UnidadeResquestDto criar, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarStatusAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}