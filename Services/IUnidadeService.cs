using System.Linq.Expressions;
using Franqueada.API.Models;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>> ObterTodasAsync(
        Nome_Unidade? nome_Unid,
        int id,
        StatusAtivo? status,
        Cod_Identificar? cod_Identificar,
        Endereco? endereco,
        CancellationToken cancellationToken
        );

    Task<UnidadeResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken);

    Task<UnidadeResponsetDto> CriarAsync(UnidadeResquestDto criar, CancellationToken cancellationToken);

    Task AtualizarStatusAsync(int id, CancellationToken cancellationToken);
}