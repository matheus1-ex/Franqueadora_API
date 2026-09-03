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
    // Responsável por adicionar as unidades

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




}