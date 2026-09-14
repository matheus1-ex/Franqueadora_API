namespace Franqueada.API.Models;
public sealed class Usuario
{
    public int Id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string? Email {get; set;}
    public string? SenhaHash {get; set;}
    public StatusAtivo Status {get; set;} = StatusAtivo.Ativado;
}