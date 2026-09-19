using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IFranqueadoraService
{
    Task<IReadOnlyCollection<FranqueadoraResponseDto>> ObterTodasAsync(string? busca, StatusAtivo? status, CancellationToken cancellationToken);
    Task<FranqueadoraResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<FranqueadoraResponseDto> CriarAsync(FranqueadoraRequestDto dto, CancellationToken cancellationToken);
    Task<bool> AtualizarAsync(int id, FranqueadoraRequestDto dto, CancellationToken cancellationToken);
    Task<bool> AlternarStatusAsync(int id, StatusAtivo status, CancellationToken cancellationToken);
    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);
}