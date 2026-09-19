using System.ComponentModel.DataAnnotations;
namespace Franqueada.API.DTOs;
public sealed class LoginRequestDto
{
    /// <summary>
    /// Campo de Id (define o tipo de usuário)
    /// </summary>
    [Required(ErrorMessage = "O Id define o tipo do usuário. ex: 1 - Admin, 2 - Gest, 3 - Usr")]
    public int IdPerfil {get; set;}

    /// <summary>
    /// Campo do Nome
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(maximumLength: 45, ErrorMessage = "O nome está inválido / cadastrado")]
    public string Nome {get; set;} = string.Empty;

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

    /// <summary>
    /// Esse campo de Data de Expiração é Opcional
    /// </summary>
    public DateTime? DataExpiracao {get; set;}
    
}