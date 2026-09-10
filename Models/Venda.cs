namespace Franqueada.API.Models;
public sealed class Venda
    {
        public int Id { get; set; }
        public int UnidadeId { get; set; }
        public Unidade? Unidade { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.UtcNow;
        public decimal ValorTotal { get; set; }

        public List<ItemVenda> Itens { get; set; } = new();
    }