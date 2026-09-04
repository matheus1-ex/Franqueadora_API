using System.Linq.Expressions;
using Franqueada_API.DTOs;
using Franqueada_API.Models;

namespace Franqueada_API.Services;

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

}