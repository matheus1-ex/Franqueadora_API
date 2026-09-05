using System.ComponentModel.DataAnnotations;

namespace Franqueada.API.DTOs;
public sealed class UsuarioCadastroDto
{
    /// <summary>
    /// Campo de Nome
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Informe seu nome para continuar."
    )]
    [StringLength(
        MinimumLength = 5,
        maximumlenght = 50,
        ErrorMessage = "No minímo 5 caracteres"
    )]

    public string Nome {get; set;} = string.Empty;

    /// <summary>
    /// Campo de Email
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Informe seu email para continuar."
    )]
    [StringLength(
        maximumLength: 250,
        ErrorMessage = "No máximo 250 caracteres."
    )]

    public string Email {get; set;} = string.Empty;

    /// <summary>
    /// Campo de Senha
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Informe sua senha para continuar."
    )]
    [StringLength(
        maximumLength: 250,
        MinimumLength = 8,
        ErrorMessage = "No minímo 8 caracteres"
    )]

    public string Senha {get; set;} = string.Empty;
}