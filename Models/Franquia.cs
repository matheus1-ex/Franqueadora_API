namespace Franqueadora.API.Models;
public sealed class Franquia
{
  public int Id_Franquia {get; set;}
  public string Nome_Marca {get; set;}
  public string Cnpj {get; set;}
  public string Status {get; set;} = StatusAtivo.Desativado;

  // Chave estrangeira para Franqueadora
  public int FranqueadoraId {get; set;}
  public Franqueadora Franqueadora {get; set;}

  // Uma Franquia possui várias unidades
  public ICollection<Unidade> Unidades {get; set;} = new List<Unidade>();
}