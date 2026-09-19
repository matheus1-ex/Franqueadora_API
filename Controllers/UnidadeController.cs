using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

namespace Franqueada_API.Controller;

[ApiController]
[Route("api/unidades")]
public class UnidadeController : ControllerBase
{
    private readonly IUnidadeService _unidadeService;

    public UnidadeController(IUnidadeService unidadeService)
    {
        _unidadeService = unidadeService;
    }

    // ========================
    // GET api/unidades/
    // ========================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadeResponseDto>>> ObterTodas(
        [FromQuery] StatusAtivo status,
        [FromQuery] string nome,
        CancellationToken cancellationToken)
    {
        IEnumerable<UnidadeResponseDto> unidades = await _unidadeService.ObterTodasAsync(
            nome_Unid: nome,
            status: status, 
            cancellationToken: cancellationToken
        );
        return Ok(unidades);
    }

    // ========================
    // GET api/unidades/{id}
    // ========================
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Unidade>> ObterId(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        UnidadeResponseDto? unidades = await _unidadeService.ObterPorIdAsync(
            id,
            cancellationToken
        );

        if (unidades is null)
        {
            return NotFound(
                new
                {
                    mensagem = $"A Unidade {id} não foi encontrada."
                }
            );
        }
        return Ok(unidades);
    }

    // ========================
    // POST api/unidades/
    // ========================
    [HttpPost]
    public async Task<ActionResult<UnidadeResponseDto>> Adicionar(
        [FromBody] List<UnidadeRequestDto> adicionarUnidade,
        CancellationToken cancellationToken
    )
    {
        if (adicionarUnidade == null || adicionarUnidade.Count == 0)
        {
            return BadRequest("Nenhum item foi enviado no corpo da requisição.");
        }

        var resultado = await _unidadeService.CriarAsync(adicionarUnidade, cancellationToken);

        return Ok(resultado);
    }

    // ===========================
    // PATCH api/unidades/{id}/status
    // ===========================
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlternarStatus(
        [FromRoute] int id,
        CancellationToken cancellationtoken
    )
    {
        await _unidadeService.AtualizarStatusAsync(id, cancellationtoken);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UnidadeResponseDto>> Atualizar(
        int id,
        [FromBody] UnidadeRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var unidadeAtualizada = await _unidadeService.AtualizarAsync(id, dto, cancellationToken);
            if (unidadeAtualizada == null)
                return NotFound(new { mensagem = $"Unidade com ID {id} não foi encontrada." });

            return Ok(unidadeAtualizada);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Remover(
        int id,
        CancellationToken cancellationToken)
    {
        bool removido = await _unidadeService.RemoverAsync(id, cancellationToken);
        if (!removido)
            return NotFound(new { mensagem = $"Unidade com ID {id} não foi encontrada." });

        return NoContent();
    }
}   