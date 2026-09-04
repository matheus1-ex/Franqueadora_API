using Franqueadora_API.Models;
using Franqueadora_API.DTOs;

namespace Franqueadora_API.Services;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(
        Nome_Unidade? nome_Unid,
        int id,
        Cod_Identificar? cod_Identificar,
        Endereco? endereco,
        CancellationToken cancellationToken
        );

    Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken);

    Task<UnidadeResponsetDto> CriarAsync(UnidadeResquestDto criar, CancellationToken cancellationToken);

    Task AtualizarStatusAsync(int id, CancellationToken cancellationToken);
}