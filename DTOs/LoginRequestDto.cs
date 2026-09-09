using System.ComponentModel.DataAnnotations;
namespace Franqueada.API.DTOs;
public sealed class LoginRequestDto
{
    /// <summary>
    /// Campo de Email
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string Email {get; set;} = string.Empty;

    /// <summary>
    /// Campo da Senha
    /// </summary>
    /// 
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha {get; set;} = string.Empty;
    
}