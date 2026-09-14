using Microsoft.AspNetCore.Mvc;
using Franqueada.API.Services;
using Franqueada.API.Models;

namespace Franqueada.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedoresController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        /// <summary>
        /// Cadastrar um novo fornecedor
        /// POST /api/fornecedores
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FornecedorResponseDto>> Criar(
            [FromBody] FornecedorRequestDto dto,
            CancellationToken cancellationToken)
        {
            var resultado = await _fornecedorService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }

        /// <summary>
        /// Obter fornecedor por ID
        /// GET /api/fornecedores/1
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FornecedorResponseDto>> ObterPorId(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var fornecedor = await _fornecedorService.ObterPorIdAsync(id, cancellationToken);

            if (fornecedor == null)
                return NotFound(new { mensagem = $"Fornecedor com ID {id} não foi encontrado." });

            return Ok(fornecedor);
        }

        /// <summary>
        /// Listar todos os fornecedores (com filtro opcional por nome ou CNPJ)
        /// GET /api/fornecedores?termo=
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorResponseDto>>> ObterTodos(
            CancellationToken cancellationToken)
        {
            var fornecedores = await _fornecedorService.ObterTodosAsync(cancellationToken);
            return Ok(fornecedores);
        }

        /// <summary>
        /// Atualizar dados de um fornecedor
        /// PUT /api/fornecedores/1
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FornecedorResponseDto>> Atualizar(
            [FromRoute] int id,
            [FromBody] FornecedorRequestDto dto,
            CancellationToken cancellationToken)
        {
            var atualizado = await _fornecedorService.AtualizarAsync(id, dto, cancellationToken);

            if (atualizado == false)
                return NotFound(new { mensagem = $"Fornecedor com ID {id} não foi encontrado." });

            return Ok(atualizado);
        }

        /// <summary>
        /// Excluir um fornecedor
        /// DELETE /api/fornecedores/1
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remover(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var removido = await _fornecedorService.RemoverAsync(id, cancellationToken);

            if (!removido)
                return NotFound(new { mensagem = $"Fornecedor com ID {id} não foi encontrado." });

            return NoContent(); // HTTP 204
        }

        /// <summary>
        /// Alternar status (Ativado / Desativado)
        /// PATCH /api/fornecedores/1/status
        /// </summary>
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> AlternarStatus(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var alterado = await _fornecedorService.AlternarStatusAsync(id, cancellationToken);

            if (!alterado)
                return NotFound(new { mensagem = $"Fornecedor com ID {id} não foi encontrado." });

            return NoContent(); // HTTP 204
        }
    }
}