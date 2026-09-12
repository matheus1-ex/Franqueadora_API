using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

namespace Franqueada.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("faturamento")]
    public async Task<ActionResult<IEnumerable<FaturamentoUnidadeDto>>> GetFaturamento(
        [FromQuery] DateTime inicio, 
        [FromQuery] DateTime fim, 
        CancellationToken cancellationToken)
    {
        var res = await _relatorioService.ObterFaturamentoPorUnidadeAsync(inicio, fim, cancellationToken);
        return Ok(res);
    }

    [HttpGet("ranking-unidades")]
    public async Task<ActionResult<IEnumerable<RankingUnidadeDto>>> GetRanking(
        [FromQuery] DateTime inicio, 
        [FromQuery] DateTime fim, 
        CancellationToken cancellationToken)
    {
        var res = await _relatorioService.ObterRankingUnidadesAsync(inicio, fim, cancellationToken);
        return Ok(res);
    }

    [HttpGet("royalties")]
    public async Task<ActionResult<ResumoRoyaltiesDto>> GetRoyalties(
        [FromQuery] int? mes, 
        [FromQuery] int? ano, 
        CancellationToken cancellationToken)
    {
        var res = await _relatorioService.ObterTotalRoyaltiesAsync(mes, ano, cancellationToken);
        return Ok(res);
    }

    [HttpGet("produtos-mais-vendidos")]
    public async Task<ActionResult<IEnumerable<ProdutoMaisVendidoDto>>> GetProdutosMaisVendidos(
        [FromQuery] int top = 10, 
        CancellationToken cancellationToken = default)
    {
        var res = await _relatorioService.ObterProdutosMaisVendidosAsync(top, cancellationToken);
        return Ok(res);
    }

    [HttpGet("estoque-critico")]
    public async Task<ActionResult<IEnumerable<EstoqueCriticoDto>>> GetEstoqueCritico(
        CancellationToken cancellationToken)
    {
        var res = await _relatorioService.ObterEstoqueCriticoAsync(cancellationToken);
        return Ok(res);
    }

    [HttpGet("chamados-status")]
    public async Task<ActionResult<IEnumerable<ChamadosPorStatusDto>>> GetChamadosPorStatus(
        CancellationToken cancellationToken)
    {
        var res = await _relatorioService.ObterQuantidadeChamadosPorStatusAsync(cancellationToken);
        return Ok(res);
    }
}