using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

namespace Franqueada.API.Controller;

[Authorize]
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
    // Responsável por buscar e filtrar a unidade por status e nome

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UnidadeResponseDto>>> ObterTodas(
        [FromQuery] StatusAtivos? status,
        [FromQuery] string? nome,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Unidade> unidades = await _unidadeService.ObterTodasAsync(
            status,
            nome,
            cancellationToken
        );
        return Ok(unidades);
    }

    // ========================
    // GET api/unidades/{id}
    // ========================
    // Responsavél por buscar unidade específica por ID

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Unidade>> ObterId(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        Unidade? unidades = await _unidadeService.ObterPorIdAsync(
            id,
            cancellationToken
        );

        if (unidades is null)
        {
            return NotFound(
                new
                {
                    mensagem = $"A Unidade {id} não foi encntrada."
                }
            );
        }
        return Ok(unidades);
    }

    // ========================
    // POST api/unidades/
    // ========================
    // Responsável por cadastrar uma nova unidade

    [HttpPost]
    public async Task<ActionResult<Unidade>> Adicionar(
        [FromBody] UnidadeRequestDto adicionarUnidade,
        CancellationToken cancellationToken
    )
    {
        var unidadeCriada = await _unidadeService.CriarAsync(adicionarUnidade);
        return CreatedAtAction(
            nameof(ObterTodas),
            new
            {
                id = unidadeCriada.Id_Und
            },
            unidadeCriada
        );
    }
  // ===========================
  // PATCH api/unidades/{id}/status
  // ===========================
  // Responsavel por alternar o status de ativo e inativo
  [HttpPatch("{id:int}/status")]
  public async Task<IActionResult> AlternarStatus(
    [FromRoute] int id,
    CancellationToken cancellationtoken
  )
  {
    var sinalVerde = await _unidadeService.AtualizarStatusAsync(id, cancellationtoken);
    if (!sinalVerde)
    {
      retrun 
    }
  }
}