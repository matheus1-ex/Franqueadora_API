namespace Franqueada.API.Models;
public sealed class Chamado
{
    public int Id { get; set; }
    public int UnidadeId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public CategoriaChamado Categoria { get; set; }
    public PrioridadeChamado Prioridade { get; set; }
    public StatusChamado Status { get; set; } = StatusChamado.Aberto;
    
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? DataEncerramento { get; set; }
    public string? ObservacoesEncerramento { get; set; }

    // Navegação
    public Unidade? Unidade { get; set; }
}