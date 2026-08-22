using System.ComponentModel.DataAnnotations;

public sealed class LoginRequestDto
{
    /// <summary>
    /// Campo de Email
    /// </summary>
    [Required(
        ErrorMessage = "Email é obrigatório"
    )]
    [StringLength(
        maximumLength: 500,
        ErrorMessage = "Até no máximo 500 caracteres, não pode deixar em branco"
    )]
    public string Email {get; set;}

    /// <summary>
    /// Campo da Senha
    /// </summary>
    /// 
    [Required(
        ErrorMessage = "Por favor, informe sua senha."
    )]

    [StringLength(
        MinimumLength = 8,
        maximumLength: 200,
        ErrorMessage = "Ops! Digite uma senha com no mínimo 8 caracteres"
    )]
    public string Senha {get; set;}
    
}