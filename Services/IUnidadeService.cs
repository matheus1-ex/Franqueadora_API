using System.Linq.Expressions;
using Franqueada.API.Models;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(
        int id,
        string? nome_Unid,
        StatusAtivo? status,
        string? cod_Identificar,
        string? endereco,
        CancellationToken cancellationToken
        );

    Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken);

    Task<UnidadeResponseDto> CriarAsync(UnidadeRequestDto criar, CancellationToken cancellationToken);

    Task AtualizarStatusAsync(int id, CancellationToken cancellationToken);
}