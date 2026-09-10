using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

namespace SeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarVenda([FromBody] CriarVendaRequestDto dto, CancellationToken ct)
        {
            try
            {
                var venda = await _vendaService.RegistrarVendaAsync(dto, ct);
                return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id }, venda);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId([FromRoute] int id, CancellationToken ct)
        {
            var venda = await _vendaService.ObterPorIdAsync(id, ct);
            if (venda == null) return NotFound(new { mensagem = "Venda não encontrada." });
            return Ok(venda);
        }
    }
}