namespace Franqueadora_API.Models;
public sealed class Unidade
{
  public int Id_Und {get; set;}
  public string Nome_Unidade {get; set;}
  public string Cod_Identificador {get; set;}
  public string Endereco {get; set;}

  //Chave estrangeira para Franquia
  public int FranquiaID {get; set;}
  public Franquia Franquia {get; set;}
}