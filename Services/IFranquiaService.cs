using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IFranquiaService
{
    Task<IReadOnlyCollection<FranquiaResponseDto>> ObterTodasAsync(string? busca, StatusAtivo? status, CancellationToken cancellationToken);
    Task<FranquiaResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<List<FranquiaResponseDto>> CriarAsync(List<FranquiaRequestDto> dto, CancellationToken cancellationToken);
    Task<bool> AtualizarAsync(int id, FranquiaRequestDto dto, CancellationToken cancellationToken);
    Task<bool> AlternarStatusAsync(int id, StatusAtivo status, CancellationToken cancellationToken);
    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);
}