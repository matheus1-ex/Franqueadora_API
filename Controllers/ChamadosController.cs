using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Franqueada.API.Services;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _chamadoService;

    public ChamadosController(IChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpPost]
    public async Task<ActionResult<ChamadoResponseDto>> Criar(
        [FromBody] CriarChamadoDto dto, 
        CancellationToken cancellationToken)
    {
        var chamado = await _chamadoService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = chamado.Id }, chamado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChamadoResponseDto>> ObterPorId(
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var chamado = await _chamadoService.ObterPorIdAsync(id, cancellationToken);
        if (chamado == null) return NotFound(new { mensagem = $"Chamado {id} não encontrado." });
        return Ok(chamado);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChamadoResponseDto>>> ObterTodos(
        [FromQuery] int? unidadeId,
        [FromQuery] StatusChamado? status,
        CancellationToken cancellationToken)
    {
        var chamados = await _chamadoService.ObterTodosAsync(unidadeId, status, cancellationToken);
        return Ok(chamados);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AtualizarStatus(
        [FromRoute] int id,
        [FromBody] AtualizarStatusChamadoDto dto,
        CancellationToken cancellationToken)
    {
        var atualizado = await _chamadoService.AtualizarStatusAsync(id, dto, cancellationToken);
        if (!atualizado) return NotFound(new { mensagem = $"Chamado {id} não encontrado." });
        return NoContent();
    }
}