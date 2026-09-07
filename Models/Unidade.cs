namespace Franqueada.API.Models;
public sealed class Unidade
{
  public int Id_Und {get; set;}
  public string Nome_Unidade {get; set;} = string.Empty;
  public string Cod_Identificador {get; set;}
  public StatusAtivo Status {get; set;} = StatusAtivo.Desativado;
  public string Endereco {get; set;} = string.Empty;

  //Chave estrangeira para Franquia
  public int FranquiaID {get; set;}
  public Franquia Franquia {get; set;}
}