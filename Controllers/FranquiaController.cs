using Franqueada.API.DTOs;
using Franqueada.API.Models;
using Franqueada.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FranquiasController : ControllerBase
{
    private readonly IFranquiaService _franquiaService;

    public FranquiasController(IFranquiaService franquiaService)
    {
        _franquiaService = franquiaService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FranquiaResponseDto>>> ObterTodas(
        [FromQuery] string? busca,
        [FromQuery] StatusAtivo? status,
        CancellationToken cancellationToken)
    {
        var resultado = await _franquiaService.ObterTodasAsync(busca, status, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FranquiaResponseDto>> ObterPorId([FromRoute] int id, CancellationToken cancellationToken)
    {
        var franquia = await _franquiaService.ObterPorIdAsync(id, cancellationToken);
        if (franquia == null)
            return NotFound(new { mensagem = $"Franquia com ID {id} não foi encontrada." });

        return Ok(franquia);
    }

    [HttpPost]
    public async Task<ActionResult<FranquiaResponseDto>> Criar([FromBody] List<FranquiaRequestDto> dto, CancellationToken cancellationToken)
    {
        var franquia = dto.Select(dto => new Franquia
        {
            Nome_Marca = dto.NomeMarca,
            Cnpj = dto.Cnpj,
            Status = dto.Status,
            FranqueadoraId = dto.FranqueadoraId
        }).ToList();

        if (dto == null || dto.Count == 0)
        {
            return BadRequest("Dados inválidos para cadastro de franquia.");
        }
        var result = await _franquiaService.CriarAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] FranquiaRequestDto dto, CancellationToken cancellationToken)
    {
        var atualizado = await _franquiaService.AtualizarAsync(id, dto, cancellationToken);
        if (!atualizado)
            return NotFound(new { mensagem = $"Franquia com ID {id} não foi encontrada." });

        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlternarStatus([FromRoute] int id, [FromQuery] StatusAtivo status, CancellationToken cancellationToken)
    {
        var sucesso = await _franquiaService.AlternarStatusAsync(id, status, cancellationToken);
        if (!sucesso)
            return NotFound(new { mensagem = $"Franquia com ID {id} não foi encontrada." });

        return Ok(sucesso);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover([FromRoute] int id, CancellationToken cancellationToken)
    {
        var removido = await _franquiaService.RemoverAsync(id, cancellationToken);
        if (!removido)
            return NotFound(new { mensagem = $"Franquia com ID {id} não foi encontrada." });

        return NoContent();
    }
}