namespace Franqueada.API.Models;
public sealed class MovimentoEstoque
    {
        public int Id { get; set; }
        public int EstoqueId { get; set; }
        public Estoque? Estoque { get; set; }
        public int Quantidade { get; set; }

        public TipoMovimentacao Tipo { get; set; } // Entrada = 1, Saida = 2
        public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;
        public string? Observacao { get; set; }
    }