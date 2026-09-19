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
using Microsoft.AspNetCore.DataProtection;

namespace Franqueada.API.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _contexto;

    private IConfiguration _configuration;

    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext contexto, IConfiguration configuration, TokenService tokenService)
    {
        _contexto = contexto;
        _tokenService = tokenService;
        _configuration = configuration;
    }


    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        // Busca usuário pelo E-mail no banco de dados
            var usuario = await _contexto.Usuarios.Include(usr => usr.Perfil).FirstOrDefaultAsync(usr => usr.Email == dto.Email, cancellationToken);

            // Valida se usuário existe
            if (usuario == null)
            {
                return new LoginResponseDto
                {
                    StatusConta = StatusAtivo.Desativado,
                    Mensagem = "E-mail ou senha inválidos."
                };
            }

            // É aqui que o id fala que tipo de perfil o usário é ex: Admin, Gestão, Usuário (padrão)
            // Na hora de registrar o usuário pode colocar o id do tipo de perfil que ele seja

            // Ex:
            /// usuário digitou 1 no campo idPerfil = Adminstrador
            /// usuário digitou 2 no campo idPerfil = Gestão (Gestor(a))
            /// usuário digitou 3 no campo idPerfil = Usuário Normal
            /// Se o usuário digitar qualquer número diferentes desses 3
            /// o usuário vai ser p

            string tipoPerfil = usuario.Perfil?.Tipo ?? usuario.Id switch
            {
                1 => "Administrador",
                2 => "Gestão",
                3 => "Usuário Padrão",
                _ => "Usuário Padrão"
            };

            var token = _tokenService.GerarToken(
            usuario.Id.ToString(), 
            usuario.Email!, tipoPerfil
        );

        var expiracao = DateTime.UtcNow.AddHours(
        double.Parse(_configuration["Jwt:ExpiracaoEmHoras"] ?? "8"));

            /// Aqui define o tipo do usuário
                    var TipoUsr = await _contexto.Usuarios
                    .Include(usr => usr.Perfil) // <-- Faz o 'JOIN' com a tabela Perfis
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

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

            return new LoginResponseDto
            {
                StatusConta = StatusAtivo.Ativado,
                Mensagem = "Login realizado com sucesso!",
                Nome = usuario.Nome,
                EmailUsuario = usuario.Email,
                TipoUsuario = tipoPerfil,
                DatadeExpiracao = expiracao,
                Token = token,
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
                Nome = dto.Nome,
                Senha = dto.Senha,
                Email = dto.Email,
                SenhaHash = senhaHash,
                IdPerfil = dto.IdPerfil,
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

    public async Task<(bool Sucesso, string Erro)> ValidarTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = _configuration["Jwt:SecretKey"] 
        ?? _configuration["Jwt:Key"] 
        ?? "k9X$mP2!vL7QnR4#T8zY1xU5cB3vA6mK";

        var key = Encoding.UTF8.GetBytes(secretKey);


    try
    {
        // Tenta validar o token com os parâmetros de segurança definidos no appsettings
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true, // Garante que tokens expirados sejam rejeitados
            ClockSkew = TimeSpan.Zero // Remove a tolerância de tempo padrão 
        }, out SecurityToken validatedToken);

        // Se passou pela validação sem lançar exceção, o token é válido
        return (true, string.Empty);
    }
    catch (Exception ex)
    {
        // Se o token expirou, foi alterado ou é inválido, cai no catch e retorna false
        return (false, ex.Message);
    }
    }
}