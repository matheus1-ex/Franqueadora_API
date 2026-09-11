using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;
public interface IChamadoService
{
    Task<ChamadoResponseDto> CriarAsync(CriarChamadoDto dto, CancellationToken cancellationToken);
    Task<ChamadoResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<ChamadoResponseDto>> ObterTodosAsync(int? unidadeId, StatusChamado? status, CancellationToken cancellationToken);
    Task<bool> AtualizarStatusAsync(int id, AtualizarStatusChamadoDto dto, CancellationToken cancellationToken);
}