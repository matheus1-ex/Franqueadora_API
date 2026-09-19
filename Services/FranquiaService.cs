using Franqueada.API.Data;
using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Franqueada.API.Services;

public class FranquiaService : IFranquiaService
{
    private readonly AppDbContext _context;

    public FranquiaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<FranquiaResponseDto>> ObterTodasAsync(string? busca, StatusAtivo? status, CancellationToken cancellationToken)
    {
        var query = _context.Franquias.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            query = query.Where(f => (f.Nome_Marca != null && f.Nome_Marca.Contains(busca)) ||
                                     (f.Cnpj != null && f.Cnpj.Contains(busca)));
        }

        if (status.HasValue)
        {
            query = query.Where(f => f.Status == status.Value);
        }

        var franquias = await query.ToListAsync(cancellationToken);

        return franquias.Select(MapToResponseDto).ToList();
    }

    public async Task<FranquiaResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var franquia = await _context.Franquias
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id_Franquia == id, cancellationToken);

        return franquia == null ? null : MapToResponseDto(franquia);
    }

    public async Task<List<FranquiaResponseDto>> CriarAsync(List<FranquiaRequestDto> dto, CancellationToken cancellationToken)
    {
        var franquia = dto.Select(dto => new Franquia
        {
            Nome_Marca = dto.NomeMarca,
            Cnpj = dto.Cnpj,
            Status = dto.Status,
            FranqueadoraId = dto.FranqueadoraId
        }).ToList();

        _context.Franquias.AddRange(franquia);
        await _context.SaveChangesAsync(cancellationToken);

        return franquia.Select(franquia => MapToResponseDto(franquia)).ToList();
    }

    public async Task<bool> AtualizarAsync(int id, FranquiaRequestDto dto, CancellationToken cancellationToken)
    {
        var franquia = await _context.Franquias.FirstOrDefaultAsync(f => f.Id_Franquia == id, cancellationToken);
        if (franquia == null) return false;

        franquia.Nome_Marca = dto.NomeMarca;
        franquia.Cnpj = dto.Cnpj;
        franquia.Status = dto.Status;
        franquia.FranqueadoraId = dto.FranqueadoraId;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> AlternarStatusAsync(int id, StatusAtivo status, CancellationToken cancellationToken)
    {
        var franquia = await _context.Franquias.FirstOrDefaultAsync(f => f.Id_Franquia == id, cancellationToken);
        if (franquia == null) return false;

        franquia.Status = status;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken)
    {
        var franquia = await _context.Franquias.FirstOrDefaultAsync(f => f.Id_Franquia == id, cancellationToken);
        if (franquia == null) return false;

        _context.Franquias.Remove(franquia);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static FranquiaResponseDto MapToResponseDto(Franquia f) => new()
    {
        Id = f.Id_Franquia,
        NomeMarca = f.Nome_Marca ?? string.Empty,
        Cnpj = f.Cnpj ?? string.Empty,
        Status = f.Status,
        FranqueadoraId = f.FranqueadoraId
    };
}