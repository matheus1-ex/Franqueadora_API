namespace Franqueas.API.Services;

public interface IFornecedorService
    {
        Task<IReadOnlyCollection<FornecedorResponseDto>> ObterTodosAsync(string? nome, string? cnpj, StatusAtivo? status, CancellationToken cancellationToken = default);
        Task<FornecedorResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<FornecedorResponseDto> CriarAsync(FornecedorRequestDto dto, CancellationToken cancellationToken = default);
        Task<bool> AtualizarAsync(int id, FornecedorRequestDto dto, CancellationToken cancellationToken = default);
        Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> AssociarAoProdutoAsync(int produtoId, int fornecedorId, CancellationToken cancellationToken = default);
        Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default);

}