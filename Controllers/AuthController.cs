using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Franqueada.API.Services;

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
        var resultado = await _authService.LoginAsync(dto, cancellationToken);

        if (!resultado.StatusConta)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<LoginResponseDto>> Registrar(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        var resultado = await _authService.RegistrarAsync(dto, cancellationToken);

        if (!resultado.StatusConta)
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
        var ehValido = await _authService.ValidarTokenAsync(token, cancellationToken);

        if (!ehValido)
        {
            return Unauthorized(new { mensagem = "Token inválido ou expirado." });
        }

        return Ok(new { valido = true });
    }
}
