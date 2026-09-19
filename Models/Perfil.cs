namespace Franqueada.API.Models;

public sealed class Perfil
{
    public int IdPerfil{get; set;}
    public string? Tipo {get; set;}

    public StatusAtivo Status { get; set; } = StatusAtivo.Ativado;
}