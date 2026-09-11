using System.ComponentModel.DataAnnotations;
using Franqueada.API.Models;

public sealed class FornecedorRequestDto
{
[Required]
public string NomeRazaoSocial { get; set; } = string.Empty;

[Required]
public string Cnpj { get; set; } = string.Empty;
public string Telefone { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
}

public sealed class FornecedorResponseDto
{
    public int Id { get; set; }
    public string NomeRazaoSocial { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public StatusAtivo Status { get; set; }
}

