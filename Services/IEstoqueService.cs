using Franqueada.API.Models;
namespace Franqueada.API.Services;
public interface IEstoqueService
{
    Task<int?> ObterSaldoAsync(int produtoId, int unidadeId, CancellationToken cancellationToken = default);
    Task<List<Estoque>> ObterItensAbaixoDoMinimoAsync(int unidadeId, int limiteMinimo, CancellationToken cancellationToken = default);
    Task MovimentarEstoqueAsync(int produtoId, int unidadeId, int quantidade, TipoMovimentacao tipo, string observacao, CancellationToken cancellationToken = default);
}