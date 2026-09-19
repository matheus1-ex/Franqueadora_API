using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Franqueada.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FranqueadorasController : ControllerBase
{
    private readonly IFranqueadoraService _franqueadoraService;

    public FranqueadorasController(IFranqueadoraService franqueadoraService)
    {
        _franqueadoraService = franqueadoraService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FranqueadoraResponseDto>>> ObterTodas(
        [FromQuery] string? busca,
        [FromQuery] StatusAtivo? status,
        CancellationToken cancellationToken)
    {
        var resultado = await _franqueadoraService.ObterTodasAsync(busca, status, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FranqueadoraResponseDto>> ObterPorId([FromRoute] int id, CancellationToken cancellationToken)
    {
        var franqueadora = await _franqueadoraService.ObterPorIdAsync(id, cancellationToken);
        if (franqueadora == null)
            return NotFound(new { mensagem = $"Franqueadora com ID {id} não foi encontrada." });

        return Ok(franqueadora);
    }

    [HttpPost]
    public async Task<ActionResult<FranqueadoraResponseDto>> Criar([FromBody] FranqueadoraRequestDto dto, CancellationToken cancellationToken)
    {
        if (dto == null)
            return BadRequest("Dados inválidos para cadastro de franqueadora.");

        var criada = await _franqueadoraService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] FranqueadoraRequestDto dto, CancellationToken cancellationToken)
    {
        var atualizado = await _franqueadoraService.AtualizarAsync(id, dto, cancellationToken);
        if (!atualizado)
            return NotFound(new { mensagem = $"Franqueadora com ID {id} não foi encontrada." });

        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlternarStatus([FromRoute] int id, [FromQuery] StatusAtivo status, CancellationToken cancellationToken)
    {
        var sucesso = await _franqueadoraService.AlternarStatusAsync(id, status, cancellationToken);
        if (!sucesso)
            return NotFound(new { mensagem = $"Franqueadora com ID {id} não foi encontrada." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover([FromRoute] int id, CancellationToken cancellationToken)
    {
        var removido = await _franqueadoraService.RemoverAsync(id, cancellationToken);
        if (!removido)
            return NotFound(new { mensagem = $"Franqueadora com ID {id} não foi encontrada." });

        return NoContent();
    }
}