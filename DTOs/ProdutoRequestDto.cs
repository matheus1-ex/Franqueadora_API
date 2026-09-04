using System.ComponentModel.DataAnnotations;

namespace Franqueada_API.DTOs;
public sealed class ProdutoRequestDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter pelo menos 100 caracteres.")]
    public string Nome {get; set;} = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Descricao {get; set;} = string.Empty;

    [Required(ErrorMessage = "O preço base é obrigatório.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public double PrecoBase {get; set;}

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(50, ErrorMessage = "A categoria deve ter no máximo 50 caracteres.")]
    public string Categoria {get; set;} = string.Empty;

    public bool Status {get; set;}
}