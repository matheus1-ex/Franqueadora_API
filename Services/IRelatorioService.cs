using Franqueada.API.DTOs;
namespace Franqueada.API.Services;
public interface IRelatorioService
    {
        Task<IEnumerable<FaturamentoUnidadeDto>> ObterFaturamentoPorUnidadeAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
        Task<IEnumerable<RankingUnidadeDto>> ObterRankingUnidadesAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
        Task<ResumoRoyaltiesDto> ObterTotalRoyaltiesAsync(int? mes, int? ano, CancellationToken cancellationToken);
        Task<IEnumerable<ProdutoMaisVendidoDto>> ObterProdutosMaisVendidosAsync(int top, CancellationToken cancellationToken);
        Task<IEnumerable<EstoqueCriticoDto>> ObterEstoqueCriticoAsync(CancellationToken cancellationToken);
        Task<IEnumerable<ChamadosPorStatusDto>> ObterQuantidadeChamadosPorStatusAsync(CancellationToken cancellationToken);
    }