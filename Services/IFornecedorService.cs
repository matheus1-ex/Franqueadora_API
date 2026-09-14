using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IFornecedorService
{
    Task<List<FornecedorResponseDto>> ObterTodosAsync(CancellationToken cancellationToken);

    Task<FornecedorResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<FornecedorResponseDto> CriarAsync(FornecedorRequestDto dto, CancellationToken cancellationToken = default);

    Task<bool> AtualizarAsync(int id, FornecedorRequestDto dto, CancellationToken cancellationToken = default);

    Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> AssociarAoProdutoAsync(int produtoId, int fornecedorId, CancellationToken cancellationToken = default);
    
    public Task<bool> RemoverAsync(int id, CancellationToken cancellationToken);
}