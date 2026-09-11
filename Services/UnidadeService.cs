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

    public async Task<UnidadeResponseDto> CriarAsync(UnidadeRequestDto criar, CancellationToken cancellationToken)
    {
        var unidade = new Unidade
            {
                Nome = criar.NomeUnidade,
                Endereco = criar.Endereco,
                Cod_Identificador = $"UNI-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                Status = StatusAtivo.Ativado
            };

            _contexto.Unidades.Add(unidade);
            await _contexto.SaveChangesAsync(cancellationToken);

            return new UnidadeResponseDto
            {
                Id_Und = unidade.Id_Und,
                Nome_Unidade = unidade.Nome,
                Endereco = unidade.Endereco,
                Cod_Identificador = unidade.Cod_Identificador,
                status = unidade.Status
            };
    }

    public async Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var unidadeDto = await _contexto.Unidades
        .AsNoTracking()
        .Where(u => u.Id_Und == id)
        .Select(u => new UnidadeResponseDto
        {
            Id_Und = u.Id_Und,
            Nome_Unidade = u.Nome,
            Endereco = u.Endereco,
            Cod_Identificador = u.Cod_Identificador,
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
                    Nome_Unidade = unidade.Nome,
                    Endereco = unidade.Endereco,
                    Cod_Identificador = unidade.Cod_Identificador,
                    status = unidade.Status
                })
                .ToListAsync(cancellationToken);
    }
}
    