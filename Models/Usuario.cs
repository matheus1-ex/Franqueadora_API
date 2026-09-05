namespace Franqueada.API.Models;
public sealed class Usuario
{
    public int Id {get; set;}
    public string Email {get; set;}
    public string SenhaHash {get; set;}
    public string Status {get; set;} = StatusAtivo.Desativado;
    public int PerfilId {get; set;}
}