namespace Franqueada.API.Models;
public sealed class Usuario
{
    public int Id {get; set;}
    public string Email {get; set;}
    public string SenhaHash {get; set;}
    public bool StatusAtivo {get; set;}
    public int PerfilId {get; set;}
}