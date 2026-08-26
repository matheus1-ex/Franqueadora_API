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
        maximumlenght = 10,
        ErrorMessage = "No minímo 5 caracteres até 10 caracteres"
    )]

    public string Nome {get; set;}

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

    public string Email {get; set;}

    /// <summary>
    /// Campo de Senha
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Informe sua senha para continuar."
    )]
    [StringLength(
        MinimumLength = 8,
        ErrorMessage = "No minímo 8 caracteres"
    )]

    public string Senha {get; set;}
}