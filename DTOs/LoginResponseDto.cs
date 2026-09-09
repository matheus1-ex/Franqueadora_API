using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Franqueada.API.Models;
namespace Franqueada.API.DTOs;
public sealed class LoginResponseDto
{
    /// <summary>
    /// Mensagem serve para dar um retorno claro sobre o resultado da tentativa de autenticação
    /// </summary>/
    public string Mensagem { get; set; } = string.Empty;

    ///<summary>
    /// Token Gerado JWT
    /// </summary>
    public string Token {get; set;} = string.Empty;

    /// <summary>
    /// Data de Expiração
    /// </summary>
    public DateTime? DatadeExpiracao {get; set;} 
    
    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string? Nome {get; set;}

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string? EmailUsuario { get; set; }

    /// <summary>
    /// Status da Conta
    /// </summary>
    public bool StatusConta {get; set;}

}