namespace Franqueada.API.Models;
public sealed class Franquia
{
  public int Id_Franquia {get; set;}
  public string? Nome_Marca {get; set;}
  public string? Cnpj {get; set;}
  public StatusAtivo Status {get; set;} = StatusAtivo.Ativado;

  // Chave estrangeira para Franqueadora
  public int FranqueadoraId {get; set;}
  public Franqueadores? Franqueadora {get; set;}

  // Uma Franquia possui várias unidades
  public ICollection<Unidade> Unidades {get; set;} = new List<Unidade>();
}