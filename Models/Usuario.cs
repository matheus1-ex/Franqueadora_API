namespace Franqueada.API.Models;
public sealed class Usuario
{
    public int Id {get; set;}
    public string? Email {get; set;}
    public string? SenhaHash {get; set;}
    public StatusAtivo Status {get; set;} = StatusAtivo.Ativado;
    public string Perfil {get; set;} = string.Empty;
}