using System.ComponentModel.DataAnnotations;
using Franqueada.API.Models;

namespace Franqueada.API.DTOs;

public class FranqueadoraRequestDto
{
    [Required]
    public string RazaoSocial { get; set; } = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public StatusAtivo? Status { get; set; }
}

public class FranqueadoraResponseDto
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativado;
}