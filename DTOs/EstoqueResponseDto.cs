using Franqueada.API.Models;
public class MovimentarEstoqueDto
    {
        public int ProdutoId { get; set; }
        public int UnidadeId { get; set; }
        public int Quantidade { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public string? Observacao { get; set; }
    }

    public sealed class EstoqueResponseDto
    {
        public int Id { get; set; }
        public int Id_Produto { get; set; }
        public string Nome_Produto { get; set; } = string.Empty;
        public int UnidadeId { get; set; }
        public string Nome_Unidade { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public int EstoqueMinimo { get; set; }
        public bool AbaixoDoMinimo => Quantidade < EstoqueMinimo;
    }