using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
namespace Franqueada.API.DTOs;
public sealed class LoginResponseDto
{
    ///<summary>
    /// Campo de Token de acesso
    /// </summary>
    [Required(
        ErrorMessage = "Por favor, digite seu código de acesso para continuar."
    )]
    [StringLength(
        MinimumLength = 19,
        ErrorMessage = "O código precisa ter pelo menos 19 dígitos. Dá uma conferida!"
    )]

    public string Token {get; set;}

    /// <summary>
    /// Campo de Data de Expiração
    /// </summary>
    [Required(
        ErrorMessage = "Este campo é obrigatório para concluir o cadastro."
    )]
    public DateTime DatadeExpiracao {get; set;} 
    
    /// <summary>
    /// Campo de Nome de Usuário
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Esse campo é obrigatório"
    )]
    [StringLength(
        MinimumLength = 5,
        maximumLenght = 10,
        ErrorMessage = "5 a 10 caracteres aceitos para esse campo"
    )]

    public string NomeUsuario {get; set;}

}