using Franqueadora.API.Models;
using Franqueadora.API.DTOs;

namespace Franqueadora.API.Services;

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

    Task AtualizarStatusAtivo(int id, CancellationToken cancellationToken);
}