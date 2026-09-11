using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Franqueada.API.Data;
using Franqueada.API.Services;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceiroController : ControllerBase
{
  private readonly IFinanceiroService _financeiroService;

  public FinanceiroController(IFinanceiroService financeiroService)
  {
    _financeiroService = financeiroService;
  }

  [HttpPost("configurar-royalty")]
  public async Task<IActionResult> DefinirPercentual([FromBody] SalvarConfiguracaoRoyaltyDto dto, CancellationToken cancellationToken)
  {
    await _financeiroService.DefinirPercentualRoyaltyAsync(dto, cancellationToken);
    return NoContent();
  }

  [HttpPost("gerar-royalty")]
  public async Task<IActionResult> GerarRoyalty([FromBody] GerarRoyaltyDto dto, CancellationToken cancellationToken)
  {
    try
    {
      var resultado = await _financeiroService.CalcularEGerarRoyaltyAsync(dto, cancellationToken);
      return Ok(resultado);
    }
    catch (Exception ex)
    {
      return BadRequest(new { mensagem = ex.Message });
    }
  }

    [HttpPatch("pagar-royalty/{id:int}")]
    public async Task<IActionResult> Pagar([FromRoute] int id, CancellationToken cancellationToken)
    {
      var status = await _financeiroService.RegistrarPagamentoAsync(id, cancellationToken);
      if (!status) return NotFound(new { mensagem = "Lançamento não encontrado." });
      return NoContent();
    }
  
    [HttpGet("royalties")]
    public async Task<IActionResult> Consultar([FromQuery] int? unidadeId, [FromQuery] StatusPagamento? status, CancellationToken cancellationToken)
    {
      var lancamentos = await _financeiroService.ConsultarValoresPorUnidadeAsync(unidadeId, status, cancellationToken);
      return Ok(lancamentos);
    }
}