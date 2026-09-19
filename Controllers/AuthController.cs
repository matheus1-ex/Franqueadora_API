using Microsoft.AspNetCore.Mvc;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Franqueada.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Franqueada.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        var resposta = await _authService.LoginAsync(dto, cancellationToken);

        if (resposta.StatusConta == StatusAtivo.Desativado)
            return Unauthorized(resposta);

        return Ok(resposta);
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<LoginResponseDto>> Registrar(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        var resultado = await _authService.RegistrarAsync(dto, cancellationToken);

        if (resultado.StatusConta == StatusAtivo.Desativado)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    [HttpPost("validar-token")]
    public async Task<ActionResult<bool>> ValidarToken(
        [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        var (sucesso, erro) = await _authService.ValidarTokenAsync(token, cancellationToken);

        if (sucesso)
        {
            return Ok(new {valido = true });
        }

        return Unauthorized(new { mensagem = "Token inválido / expirado", detalhe = erro });
    }

    // Busca pelo Id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoginResponseDto>> ObterPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var usuario = await _authService.ObterPorIdAsync(id, cancellationToken);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    // Remover o usuario pelo id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoverUsuarioAsync(
        int id,
        CancellationToken cancellationToken)
    {
        bool removido = await _authService.RemoverUsuarioAsync(
            id,
            cancellationToken
        );
        if (!removido)
        {
            return NotFound(new { mensagem = $"O Usuário com {id} não foi encontrado"} );
        }
        return NoContent();
    }

}
