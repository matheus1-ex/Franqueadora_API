using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

namespace SeuProjeto.Controllers
{
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
        public async Task<IActionResult> ObterSaldo([FromQuery] int produtoId, [FromQuery] int unidadeId, CancellationToken ct)
        {
            var saldo = await _estoqueService.ObterSaldoAsync(produtoId, unidadeId, ct);
            if (saldo == null) return NotFound(new { mensagem = "Registro de estoque não encontrado." });
            return Ok(saldo);
        }

        [HttpGet("abaixo-minimo")]
        public async Task<IActionResult> ObterAbaixoMinimo([FromQuery] int? unidadeId, CancellationToken ct)
        {
            var itens = await _estoqueService.ObterItensAbaixoDoMinimoAsync(unidadeId, ct);
            return Ok(itens);
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentarEstoqueDto dto, CancellationToken ct)
        {
            try
            {
                await _estoqueService.MovimentarEstoqueAsync(dto, ct);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}