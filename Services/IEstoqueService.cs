namespace Franqueada.API.Services;
public interface IEstoqueService
    {
        Task<EstoqueResponseDto?> ObterSaldoAsync(int produtoId, int unidadeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<EstoqueResponseDto>> ObterItensAbaixoDoMinimoAsync(int? unidadeId, CancellationToken cancellationToken = default);
        Task<bool> MovimentarEstoqueAsync(MovimentarEstoqueDto dto, CancellationToken cancellationToken = default);
    }