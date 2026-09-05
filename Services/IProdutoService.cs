using System.Linq.Expressions;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IProdutoService
{
    Task<IReadOnlyCollection<ProdutoResponseDto>> ObterTodasAsync (
        string? nome,
        string? categoria,
        bool status,
        CancellationToken cancellationToken = default
    );

    Task<ProdutoResponseDto?> ObterIdAsync (int id, CancellationToken cancellationToken = default);

    Task<ProdutoResponseDto?> CriarAsync (ProdutoRequestDto dto, CancellationToken cancellationToken = default);

    Task<bool> AtualizarAsync (int id, ProdutoRequestDto dto, CancellationToken cancellationToken = default);

    Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken = default); 

    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default);

}