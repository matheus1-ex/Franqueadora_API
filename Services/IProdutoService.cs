using System.Linq.Expressions;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IProdutoService
{
    public Task<IReadOnlyCollection<ProdutoResponseDto>> ObterTodasAsync (
        string? nome,
        string? busca,
        string? categoria,
        StatusAtivo? status,
        CancellationToken cancellationToken
    );

    public Task<ProdutoResponseDto?> ObterIdAsync (int id, CancellationToken cancellationToken);

    public Task<ProdutoResponseDto?> CriarAsync (ProdutoRequestDto dto, CancellationToken cancellationToken);

    public Task<ProdutoResponseDto?> AtualizarAsync (int id, ProdutoRequestDto dto, CancellationToken cancellationToken);

    public Task<bool> AlternarStatusAsync(int id, StatusAtivo status,CancellationToken cancellationToken); 

    public Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);

}