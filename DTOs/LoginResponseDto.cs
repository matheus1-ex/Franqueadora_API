using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
namespace Franqueada_API.DTOs;
public sealed class LoginResponseDto
{
    ///<summary>
    /// Token Gerado JWT
    /// </summary>
    /// 
    public string Token {get; set;}

    /// <summary>
    /// Data de Expiração
    /// </summary>
    public DateTime? DatadeExpiracao {get; set;} 
    
    /// <summary>
    /// Usuário
    /// </summary>
    public string Nome {get; set;}

    [EnumDataType(typeof(StatusAtivos), ErrorMessage = "Status do usuário inválido.")]
    public StatusAtivos statusAtivos {get; set;}

}