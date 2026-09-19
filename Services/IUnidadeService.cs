using System.Linq.Expressions;
using Franqueada.API.Models;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(
        string? nome_Unid,
        StatusAtivo? status,
        CancellationToken cancellationToken
        );

    Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken);

    Task<List<UnidadeResponseDto>> CriarAsync(List<UnidadeRequestDto> criar, CancellationToken cancellationToken);

    Task AtualizarStatusAsync(int id, CancellationToken cancellationToken);

    Task<UnidadeResponseDto?> AtualizarAsync(int id, UnidadeRequestDto dto, CancellationToken cancellationToken);
    
    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);
}