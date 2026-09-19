using Microsoft.EntityFrameworkCore;
using Franqueada.API.Models;
using Franqueada.API.Data;
using Franqueada.API.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Franqueada.API.Services;

public class UnidadeService : IUnidadeService
{
    private readonly AppDbContext _contexto;

    public UnidadeService(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    private static UnidadeResponseDto MapToDto(Unidade unidade)
  {
      return new UnidadeResponseDto
      {
          Id_Und = unidade.Id_Und,
          NomeUnidade = unidade.Nome,
          Cidade = unidade.Cidade,
          Estado = unidade.Estado,
          Telefone = unidade.Telefone,
          Endereco = unidade.Endereco,
          CodIdentificador = unidade.Cod_Identificador,
          FranquiaId = unidade.FranquiaId,
          status = StatusAtivo.Ativado,
      };
  }

    public async Task<UnidadeResponseDto?> AtualizarAsync(int id, UnidadeRequestDto dto, CancellationToken cancellationToken)
    {
        var unidade = await _contexto.Unidades.FirstOrDefaultAsync(u => u.Id_Und == id, cancellationToken);
        if (unidade == null) return null;

        // Valida se a franquia existe antes de atualizar
        bool franquiaExiste = await _contexto.Franquias.AnyAsync(f => f.Id_Franquia == dto.FranquiaId, cancellationToken);
        if (!franquiaExiste)
        {
            throw new Exception($"A franquia com ID {dto.FranquiaId} não existe.");
        }

        // Mapeamento dos campos
        unidade.Nome = dto.NomeUnidade;
        unidade.Cidade = dto.Cidade;
        unidade.Estado = dto.Estado;
        unidade.Telefone = dto.Telefone;
        unidade.Endereco = dto.Endereco;
        unidade.FranquiaId = dto.FranquiaId;
        unidade.Status = StatusAtivo.Ativado;

        await _contexto.SaveChangesAsync(cancellationToken);

        return new UnidadeResponseDto
        {
            Id_Und = unidade.Id_Und,
            NomeUnidade = unidade.Nome,
            Cidade = unidade.Cidade,
            Estado = unidade.Estado,
            Telefone = unidade.Telefone,
            Endereco = unidade.Endereco,
            FranquiaId = unidade.FranquiaId,
            status = StatusAtivo.Ativado,
        };
    }

    public async Task AtualizarStatusAsync(int id, CancellationToken cancellationToken)
    {
        var unidade = await _contexto.Unidades.FirstOrDefaultAsync(u => u.Id_Und == id, cancellationToken);

            if (unidade == null)
            {
                throw new KeyNotFoundException($"Unidade com ID {id} não encontrada.");
            }

            // Alterna entre Ativado e Desativado
            unidade.Status = unidade.Status == StatusAtivo.Ativado 
                ? StatusAtivo.Desativado 
                : StatusAtivo.Ativado;

            await _contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UnidadeResponseDto>> CriarAsync(List<UnidadeRequestDto> criar, CancellationToken cancellationToken)
    {
        var unidade = criar.Select(criar => new Unidade
            {
                Nome = criar.NomeUnidade,
                Endereco = criar.Endereco,
                Cidade = criar.Cidade,       
                Estado = criar.Estado,       
                Telefone = criar.Telefone,
                Cod_Identificador = $"UNI-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                FranquiaId = criar.FranquiaId,
                Status = StatusAtivo.Ativado
            }).ToList();

            _contexto.Unidades.AddRange(unidade);
            await _contexto.SaveChangesAsync(cancellationToken);

            return unidade.Select(unidade =>MapToDto(unidade)).ToList();
    }

    public async Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var unidadeDto = await _contexto.Unidades
        .AsNoTracking()
        .Where(u => u.Id_Und == id)
        .Select(u => new UnidadeResponseDto
        {
            Id_Und = u.Id_Und,
            NomeUnidade = u.Nome,
            Cidade = u.Cidade,
            Estado = u.Estado,
            Telefone = u.Telefone,
            CodIdentificador = u.Cod_Identificador,
            Endereco = u.Endereco,
            FranquiaId = u.FranquiaId,
            status = u.Status
        })
        .FirstOrDefaultAsync(cancellationToken);

        if (unidadeDto == null)
        {
            throw new KeyNotFoundException($"Unidade com ID {id} não foi encontrada.");
        }

        return unidadeDto;

        
    }

    public async Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(string? nome_Unid, StatusAtivo? status, CancellationToken cancellationToken)
    {
        IQueryable<Unidade> query = _contexto.Unidades.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome_Unid))
            {
                query = query.Where(u => u.Nome.Contains(nome_Unid));
            }

            if (status.HasValue)
            {
                query = query.Where(u => u.Status == status.Value);
            }

            return await query
                .Select(unidade => new UnidadeResponseDto
                {
                    Id_Und = unidade.Id_Und,
                    NomeUnidade = unidade.Nome,
                    Cidade = unidade.Cidade,
                    Estado = unidade.Estado,
                    Telefone = unidade.Telefone,
                    Endereco = unidade.Endereco,
                    CodIdentificador = unidade.Cod_Identificador,
                    FranquiaId = unidade.FranquiaId,
                    status = unidade.Status
                })
                .ToListAsync(cancellationToken);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken)
    {
        var unidade = await _contexto.Unidades.FirstOrDefaultAsync(u => u.Id_Und == id, cancellationToken);
        if (unidade == null) return false;

        _contexto.Unidades.Remove(unidade);
        await _contexto.SaveChangesAsync(cancellationToken);
        return true;
    }
}
    