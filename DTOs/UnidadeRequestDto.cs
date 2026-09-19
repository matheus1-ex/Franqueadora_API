using System.ComponentModel.DataAnnotations;

namespace Franqueada.API.DTOs;
public sealed class UnidadeRequestDto
{
    /// <summary>
    /// Campo do nome de unidade
    /// </summary>
    [Required(ErrorMessage = "Esse campo é obrigatório.")]
    [StringLength(maximumLength: 100, ErrorMessage = "Digite o nome da unidade com até 100 caracteres.")]
    public string NomeUnidade {get; set;} = string.Empty;

    [Required(ErrorMessage = "Digite o nome da cidade, onde sua loja deseja vender. ")]
    [StringLength(maximumLength: 100)]
    public string Cidade {get; set;} = string.Empty;

    [Required(ErrorMessage = "Digite o Estado da sua cidade")]
    [StringLength(maximumLength: 100)]
    public string Estado {get; set;} = string.Empty;

    [Required(ErrorMessage = "Digite o número de telefone da sua loja")]
    [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-\d{4}$", ErrorMessage = "Informe um telefone no formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX.")]
    public string Telefone {get; set;} = string.Empty;


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
    public int FranquiaId {get; set;}
}