namespace Franqueada.API.Services;

using Franqueada.API.Data;
using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Microsoft.EntityFrameworkCore;

public class FranqueadoraService : IFranqueadoraService
{
    private readonly AppDbContext _context;

    public FranqueadoraService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<FranqueadoraResponseDto>> ObterTodasAsync(string? busca, StatusAtivo? status, CancellationToken cancellationToken)
    {
        var query = _context.Franqueadoras.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            query = query.Where(f => (f.Razao_Social != null && f.Razao_Social.Contains(busca)) ||
                                     (f.Cnpj != null && f.Cnpj.Contains(busca)));
        }

        if (status.HasValue)
        {
            query = query.Where(f => f.Status == status.Value);
        }

        var franqueadores = await query.ToListAsync(cancellationToken);

        return franqueadores.Select(MapToResponseDto).ToList();
    }

    public async Task<FranqueadoraResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var franqueadora = await _context.Franqueadoras
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id_Franqueadora == id, cancellationToken);

        return franqueadora == null ? null : MapToResponseDto(franqueadora);
    }

    public async Task<FranqueadoraResponseDto> CriarAsync(FranqueadoraRequestDto dto, CancellationToken cancellationToken)
    {
        var franqueadora = new Franqueadores
        {
            Razao_Social = dto.RazaoSocial,
            Cnpj = dto.Cnpj,
            Email = dto.Email,
            Status = StatusAtivo.Ativado
        };

        _context.Franqueadoras.Add(franqueadora);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToResponseDto(franqueadora);
    }

    public async Task<bool> AtualizarAsync(int id, FranqueadoraRequestDto dto, CancellationToken cancellationToken)
    {
        var franqueadora = await _context.Franqueadoras.FirstOrDefaultAsync(f => f.Id_Franqueadora == id, cancellationToken);
        if (franqueadora == null) return false;

        franqueadora.Razao_Social = dto.RazaoSocial;
        franqueadora.Email = dto.Email;
        franqueadora.Cnpj = dto.Cnpj;
        franqueadora.Status = StatusAtivo.Ativado;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> AlternarStatusAsync(int id, StatusAtivo status, CancellationToken cancellationToken)
    {
        var franqueadora = await _context.Franqueadoras.FirstOrDefaultAsync(f => f.Id_Franqueadora == id, cancellationToken);
        if (franqueadora == null) return false;

        franqueadora.Status = status;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken)
    {
        var franqueadora = await _context.Franqueadoras.FirstOrDefaultAsync(f => f.Id_Franqueadora == id, cancellationToken);
        if (franqueadora == null) return false;

        _context.Franqueadoras.Remove(franqueadora);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static FranqueadoraResponseDto MapToResponseDto(Franqueadores f) => new()
    {
        Id = f.Id_Franqueadora,
        RazaoSocial = f.Razao_Social ?? string.Empty,
        Email = f.Email ?? string.Empty,
        Cnpj = f.Cnpj ?? string.Empty,
        Status = f.Status
    };
}