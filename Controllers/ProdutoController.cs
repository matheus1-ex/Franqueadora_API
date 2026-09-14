using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using System.Linq;
using Franqueada.API.Services;
using Franqueada.API.Models;
using Franqueada.API.Data;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    
    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// 1. Consulta dinâmica por nome, categoria e status
    /// GET /api/produtos?nome=corte&categoria=servico&status=Ativado
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProdutoResponseDto>>> ObterTodas(
        [FromQuery] string? nome,
        [FromQuery] string? categoria,
        [FromQuery] string? busca,
        [FromQuery] StatusAtivo? status,
        CancellationToken cancellationToken
    )
    {
        var produtos = await _produtoService.ObterTodasAsync(nome, categoria, busca, status, cancellationToken);
        return Ok(produtos);
    }

    /// <summary>
    /// 2. Obter produto por ID
    /// GET /api/produtos/1
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> ObterPorId(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var produto = await _produtoService.ObterIdAsync(id, cancellationToken);
        if (produto == null)
        {
            return NotFound(new {mensagem = $"Produto com ID {id} não foi encontrado."});
        }
        return Ok(produto);
    }

    /// <summary>
    /// 3. Criar produto
    /// POST /api/produtos
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Criar(
        [FromBody] List<ProdutoRequestDto> dto,
        CancellationToken cancellationToken
    )
    {
        var produtos = dto.Select(dto => new Produto
        {
            NomeProduto = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.PrecoBase,
            Categoria = dto.Categoria,
            Status = StatusAtivo.Ativado,
            QuantidadeEstoque = dto.QuantidadeEstoque,
            FornecedorId = dto.FornecedorID
        }).ToList();

        if (dto == null || dto.Count == 0)
        {
            return BadRequest("Nenhum produto foi enviado no corpo da requisição.");
        }

        var resultado = await _produtoService.CriarAsync(dto, cancellationToken);

        return Ok(resultado);
    }

    /// <summary>
    /// 4. Atualizar produto completo
    /// PUT /api/produtos/1
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        [FromBody] ProdutoRequestDto dto,
        CancellationToken cancellationToken
    )
    {
        var atualizado = await _produtoService.AtualizarAsync(id, dto, cancellationToken);
        if (atualizado == null)
        {
            return NotFound(new { mensagem = $"Produto com ID {id} não foi encontrado."});
        }
        return NoContent();
    }

    /// <summary>
    /// 5. Alternar status (Ativado / Desativado)
    /// PATCH /api/produtos/1/status
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlternarStatus(
        [FromRoute] int id,
        [FromQuery] StatusAtivo status,
        CancellationToken cancellationToken
    )
    {
        var sucesso = await _produtoService.AlternarStatusAsync(id, status, cancellationToken);
        if (!sucesso)
        {
            return NotFound(new {mensagem = $"Produto com ID {id} não foi encontrado."});
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var removido = await _produtoService.RemoverAsync(id, cancellationToken);
        if (!removido)
        {
            return NotFound(new {mensagem = $"Produto com ID {id} não foi encontrado."});
        }
        return NoContent();
    }
}