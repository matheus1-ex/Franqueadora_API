namespace Franqueadora.API.Models;
public sealed class Franqueadora
{
  public int Id_Franqueadora {get; set;}
  public string Razao_Social {get; set;}
  public string Cnpj {get; set;}
  public string Status {get; set;} = StatusAtivo.Desativado;

  // Uma Franqueadora possui varias franquias
  public ICollection<Franquia> Franquias {get; set; } = new List<Franquia>();
  
}