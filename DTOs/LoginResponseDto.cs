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


}