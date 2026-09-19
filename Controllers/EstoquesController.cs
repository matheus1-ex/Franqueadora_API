using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstoquesController : ControllerBase
{
    private readonly IEstoqueService _estoqueService;

    public EstoquesController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    [HttpGet("saldo")]
    public async Task<IActionResult> ObterSaldo(
        [FromQuery] int produtoId, 
        [FromQuery] int unidadeId, 
        CancellationToken cancellationToken)
    {
        var saldo = await _estoqueService.ObterSaldoAsync(produtoId, unidadeId, cancellationToken);
        
        if (saldo == null) 
            return NotFound(new { mensagem = "Registro de estoque não encontrado." });

        return Ok(new { produtoId, unidadeId, saldo });
    }

    [HttpGet("abaixo-minimo")]
    public async Task<IActionResult> ObterAbaixoMinimo(
        [FromQuery] int? unidadeId, 
        CancellationToken cancellationToken)
    {
        var itens = await _estoqueService.ObterItensAbaixoDoMinimoAsync(
            unidadeId ?? 0, 
            5,
            cancellationToken
        );
        return Ok(itens);
    }

    [HttpPost("movimentar")]
    public async Task<IActionResult> Movimentar(
        [FromBody] MovimentarEstoqueDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            await _estoqueService.MovimentarEstoqueAsync(
                dto.ProdutoId,
                dto.UnidadeId,
                dto.Quantidade,
                dto.Tipo,
                dto.Observacao?? string.Empty,
                cancellationToken
            );
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}