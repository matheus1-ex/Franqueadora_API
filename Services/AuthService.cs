using Franqueada.API.DTOs;
using Franqueada.API.Data;
using Franqueada.API.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using BCryptNet;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Franqueada.API.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _contexto;

    private IConfiguration _configuration;

    public AuthService(AppDbContext contexto, IConfiguration configuration)
    {
        _contexto = contexto;
        _configuration = configuration;
    }


    public string GerarJwtToken(string usuarioId, string email, string perfil, DateTime expiracao)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    
    // Sua chave secreta usada para assinar o token
    var secretKey = _configuration["Jwt:SecretKey"] ?? "k9X$mP2!vL7QnR4#T8zY1xU5cB3vA6mK";
    var chaveSecreta = Encoding.ASCII.GetBytes(secretKey);

    // Define as informações contidas no Token (Claims)
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, perfil)
        }),
        Expires = expiracao,
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(chaveSecreta), 
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    // Cria e assina o token
    var token = tokenHandler.CreateToken(tokenDescriptor);
    
    // Converte para a string final que é enviada ao cliente
    return tokenHandler.WriteToken(token);
}

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        // Busca usuário pelo E-mail no banco de dados
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(usr => usr.Email == dto.Email, cancellationToken);

            // Valida se usuário existe
            if (usuario == null)
            {
                return new LoginResponseDto
                {
                    StatusConta = StatusAtivo.Desativado,
                    Mensagem = "E-mail ou senha inválidos."
                };
            }

            // Valida a Hash da Senha (BCrypt)
            bool senhaValida = BCrypt.Verify(dto.Senha, usuario.SenhaHash);
            
            if (!senhaValida)
            {
                return new LoginResponseDto
                {
                    StatusConta = StatusAtivo.Desativado,
                    Mensagem = "E-mail ou senha inválidos."
                };
            }

            // Se o usuário estiver desativado
            if (usuario.Status != StatusAtivo.Ativado)
            {
                return new LoginResponseDto
                {
                    StatusConta = StatusAtivo.Desativado,
                    Mensagem = "Usuário inativo no sistema."
                };
            }

            // Gerar Token JWT e definir expiração
            var expiracao = DateTime.UtcNow.AddHours(
                double.Parse(_configuration["Jwt:ExpiracaoEmHoras"] ?? "8"));

            var token = GerarJwtToken(usuario.Id.ToString(), usuario.Email!, usuario.Nome, expiracao);

            return new LoginResponseDto
            {
                StatusConta = StatusAtivo.Ativado,
                Mensagem = "Login realizado com sucesso!",
                Token = token,
                DatadeExpiracao = expiracao,
                Nome = usuario.Nome,
                EmailUsuario = usuario.Email
            };
        }

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _contexto.Usuarios.AsNoTracking().FirstOrDefaultAsync(
            usr => usr.Id == id,
            cancellationToken
        );
    }

    public async Task<LoginResponseDto> RegistrarAsync(LoginRequestDto dto, CancellationToken cancellationToken)
        {
            // Valida se já existe usuário cadastrado com o mesmo e-mail
            var usuarioExiste = await _contexto.Usuarios.AnyAsync(usr => usr.Email == dto.Email, cancellationToken);

            if (usuarioExiste)
            {
                return new LoginResponseDto
                {
                    StatusConta = StatusAtivo.Desativado,
                    Mensagem = "Já existe um usuário cadastrado com este e-mail."
                };
            }

            // Criptografa a senha com BCrypt antes de salvar no banco
            string senhaHash = BCrypt.HashPassword(dto.Senha);

            var novoUsuario = new Usuario
            {
                Id = dto.id,
                Nome = dto.Nome,
                Senha = dto.Senha,
                Email = dto.Email,
                SenhaHash = senhaHash,
                Token = dto.Token,
                Status = StatusAtivo.Ativado,
            };

            _contexto.Usuarios.Add(novoUsuario);
            await _contexto.SaveChangesAsync(cancellationToken);

            return await LoginAsync(dto, cancellationToken);
    }

    public async Task<bool> RemoverUsuarioAsync(int id, CancellationToken cancellationToken)
    {
        Usuario? usuario = await _contexto.Usuarios.FirstOrDefaultAsync(
            usuario => usuario.Id == id, cancellationToken
        );
        if (usuario == null)
        {
            return false;
        }
        _contexto.Usuarios.Remove(usuario);
        await _contexto.SaveChangesAsync(cancellationToken);
        return true;

    }

    public async Task<bool> ValidarTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        // Se o token for nulo ou vazio, já é inválido
    if (string.IsNullOrWhiteSpace(token))
        return false;

    var secretKey = _configuration["Jwt:SecretKey"];

    if (string.IsNullOrEmpty(secretKey))
        return false;

    var key = Encoding.ASCII.GetBytes(secretKey);

    try
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        // Tenta validar o token com os parâmetros de segurança definidos no appsettings
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true, // Garante que tokens expirados sejam rejeitados
            ClockSkew = TimeSpan.Zero // Remove a tolerância de tempo padrão de 5 min para expiração
        }, out SecurityToken validatedToken);

        // Se passou pela validação sem lançar exceção, o token é válido
        return true;
    }
    catch
    {
        // Se o token expirou, foi alterado ou é inválido, cai no catch e retorna false
        return false;
    }
    }
}