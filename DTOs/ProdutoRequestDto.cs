using System.ComponentModel.DataAnnotations;
using Franqueada.API.Models;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Franqueada.API.DTOs;
public sealed class ProdutoRequestDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter pelo menos 100 caracteres.")]
    public string Nome {get; set;} = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Descricao {get; set;} = string.Empty;

    [Required(ErrorMessage = "O preço base é obrigatório.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal PrecoBase {get; set;}

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(50, ErrorMessage = "A categoria deve ter no máximo 50 caracteres.")]
    public string Categoria {get; set;} = string.Empty;

    [Required(ErrorMessage = "A Quantidade tem que ser maior do que 0")]
    public int QuantidadeEstoque {get; set;} 

    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe o ID do fornecedor válido.")]
    public int FornecedorID {get; set;}

    public StatusAtivo? Status {get; set;}
}