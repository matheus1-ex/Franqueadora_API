using System.Linq.Expressions;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IProdutoService
{
    Task<IReadOnlyCollection<ProdutoResponseDto>> ObterTodasAsync (
        string? nome,
        string? categoria,
        StatusAtivo status,
        CancellationToken cancellationToken
    );

    Task<ProdutoResponseDto?> ObterIdAsync (int id, CancellationToken cancellationToken);

    Task<ProdutoResponseDto?> CriarAsync (ProdutoRequestDto dto, CancellationToken cancellationToken);

    Task<ProdutoResponseDto?> AtualizarAsync (int id, ProdutoRequestDto dto, CancellationToken cancellationToken);

    Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken); 

    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);

}