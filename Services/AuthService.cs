using Franqueada_API.DTOs;
using Franqueada_API.Data;
using Franqueada_API.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.AccessControl;

namespace Franqueada_API.Services;

public class AuthService : IAuthService
{
    private readonly AppContext _contexto;

    public AuthService(AppContext context)
    {
        _contexto = contexto;
    }

    public string GerarToken(Usuario usuario)
    {
        // Instancia o manipulador (handler)
        var token = new JwtSecurityTokenHandler();

        // Prepara a chave secreta em bytes
        var chaveSecreta = Encoding.ASCII.GetBytes("k9X7#m2P$vL4R8qW1zT");

        var tokenDescriptor = new SecurityDescriptor
        {
            // Identidade do usuário contida dentro do token
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.PerfilId, usuario.PerfilId.Nome)
            }),

            // Tempo de Expiração
            Expirar = DateTime.UtcNow.AddHours(15),

            // Algoritmo de criptografia
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chaveSecreta),
                SecurityAlgorithms.HmacSha256Signature
            )

        };

        // Instancia o objeto do token a partir das configurações (descriptor)
        var token_Seguro = token.CreateToken(tokenDescriptor);

        // Converte o objeto em uma string final e retorna
        string tokenString = token.WriteToken(token_Seguro);

        return tokenString;
    }

    // Validação de Conta
    public async Task<LoginResponseDto> AuntenticarAsync(LoginRequestDto requestDto)
    {
        // Validar se o email existe no banco
        var usuario = await contexto.Usuarios
        .Include(usr => usr.Perfil)
        .FirstOrDefaultAsync(usr => usr.Email == requestDto.Email);

        if (usuario == null)
        {
            throw new Exception("Email ou Senha inválidos.");
        }

        // Validar se a conta está ativa
        if (!usuario.StatusAtivo)
        {
            throw new Exception("Esta conta de usuário está inativo. Por favor, entre em contato com o admin");
        }

        // Validar a senha
        bool senha = BCrypt.Net.BCrypt.Verify(requestDto.Senha, usuario.SenhaHash);
        if (!senha)
        {
            throw new Exception("Senha Inválida.");
        }

        // Gerar o Token JWT se passar de todas as validações
        string tokenValido = GerarToken(usuario);

        // Para Criação do Novo Cadastro
        bool emailExiste = await _contexto.Usuarios.AnyAsync(usr => usr.Email == dto.Email);

        if (emailExiste)
        {
            throw new Exception("Este e-mail já está em uso por outro usuário.");
        }

        return new LoginResponseDto
        {
            Token = token,
            DataExpiracao = DateTime.UtcNow.AddHours(15)
        };
    }

}