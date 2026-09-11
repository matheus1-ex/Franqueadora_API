using Franqueada.API.Models;
namespace Franqueada.API.DTOs;
public sealed class CriarChamadoDto
{
    public int UnidadeId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public CategoriaChamado Categoria { get; set; }
    public PrioridadeChamado Prioridade { get; set; }
}

public sealed class AtualizarStatusChamadoDto
{
    public StatusChamado NovoStatus { get; set; }
    public string? Observacao { get; set; }
}

public sealed class ChamadoResponseDto
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public string NomeUnidade { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public CategoriaChamado Categoria { get; set; }
    public PrioridadeChamado Prioridade { get; set; }
    public StatusChamado Status { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataEncerramento { get; set; }
    public string? ObservacoesEncerramento { get; set; }
}