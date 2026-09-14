namespace Franqueada.API.Models;
public sealed class Produto
{
  public int Id_Produto {get; set;}
  public string NomeProduto {get; set;} = string.Empty;
  public string? Descricao {get; set;}
  public decimal Preco {get; set;}
  public string? Categoria {get; set;}
  public StatusAtivo Status {get; set;} = StatusAtivo.Ativado;

  // Produto no estoque
  public int QuantidadeEstoque {get; set;}
  public int QuantidadeMinima {get; set;}

  // Relacinameto com fornecedor
  public Fornecedor? Fornecedor {get; set;}
  public int FornecedorId {get; set;}
}