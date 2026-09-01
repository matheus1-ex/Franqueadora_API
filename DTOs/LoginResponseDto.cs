using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
namespace Franqueada.API.DTOs;
public sealed class LoginResponseDto
{
    ///<summary>
    /// Token Gerado JWT
    /// </summary>
    /// 
    public string Token {get; set;}

    /// <summary>
    /// Campo de Data de Expiração
    /// </summary>
    [Required(
        ErrorMessage = "Este campo é obrigatório para concluir o cadastro."
    )]
    public DateTime DatadeExpiracao {get; set;} 
    
    /// <summary>
    /// Campo de Usuário
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Esse campo é obrigatório."
    )]
    [StringLength(
        maximumLenght = 50,
        ErrorMessage = "Esse campo é obigatório."
    )]

    public string Nome {get; set;}

}