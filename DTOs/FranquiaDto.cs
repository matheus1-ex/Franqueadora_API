using Franqueada.API.Models;

namespace Franqueada.API.DTOs;

public class FranquiaRequestDto
{
    public string NomeMarca { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativado;
    public int FranqueadoraId { get; set; }
}

public class FranquiaResponseDto
{
    public int Id { get; set; }
    public string NomeMarca { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public StatusAtivo Status { get; set; }
    public int FranqueadoraId { get; set; }
}