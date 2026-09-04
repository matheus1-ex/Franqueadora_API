using System.ComponentModel.DataAnnotations;

namespace Franqueada_API.DTOs;
public sealed class UnidadeRequestDto
{
    /// <summary>
    /// Campo do nome de unidade
    /// </summary>
    [Required(ErrorMessage = "Esse campo é obrigatório.")]
    [StringLength(maximumLength: 100, ErrorMessage = "Digite o nome da unidade com até 100 caracteres.")]
    public string NomeUnidade {get; set;} = string.Empty;


    /// <summary>
    /// Campo de endereço
    /// </summary>
    [Required(ErrorMessage = "Esse campo é obrigatório.")]
    [StringLength(maximumLength: 250, ErrorMessage = "Digite o endereço com até 250 caracteres.")]
    public string Endereco {get; set;} = string.Empty;

    /// <summary>
    /// Campo do ID da franquia
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe o ID da franquia válido.")]
    public int FranquiaId {get; set;} = int.Empty;
}