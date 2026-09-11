
namespace Franqueada.API.Models;
public sealed class Estoque
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int UnidadeId { get; set; }
    public Unidade? Unidade { get; set; }

    public int Quantidade { get; set; }
    public int EstoqueMinimo { get; set; }
}