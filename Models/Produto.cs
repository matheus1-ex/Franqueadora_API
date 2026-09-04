namespace Franqueadora_API.Models;
public sealed class Produto
{
  public int Id_Produto {get; set;}
  public string NomeProduto {get; set;}
  public string Descricao {get; set;}
  public double Preco {get; set;}
  public string Categoria {get; set;}
  public bool Status {get; set;} = true;
}