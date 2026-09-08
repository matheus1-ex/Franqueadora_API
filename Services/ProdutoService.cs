using Microsoft.EntityFrameworkCore;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Franqueada.API.Data;


namespace Franqueada.API.Services;

public class ProdutoService : IProdutoService
{
  public readonly AppContext _contexto;
  
  public ProdutoService(AppContext contexto)
  {
    _contexto = contexto;
  }

  public async Task<ProdutoResponseDto> ObterTodasAsync (
        string? nome,
        string? categoria,
        StatusAtivo status,
        CancellationToken cancellationToken = default
  )
  {
    // Inicia a consulta como IQueryable 
    var query = _contexto.Produtos.AsNoTracking().AsQueryable();

    // Aplica o filtro de Nome se foi informado 
    if (!string.IsNullOrWhiteSpace(nome))
      query = query.Where(produto => produto.nome.ToLower().Contains(nome.ToLower()));
      
    // Aplica o filtro de Categoria se foi informado
    if (!string.IsNullOrWhiteSpace(categoria))
    {
      query = query.Where(produto => produto.Categoria.ToLower() == categoria.ToLower());
    };

    // Aplica o filtro de Status se foi informado
    if (status.HasValue)
      {
        query = query.Where(produto => produto.Status == status.Value);
      }

    // Busca pelo Nome ou Categoria
    if (!string.IsNullOrWhiteSpace(busca))
      string termo = busca.Trim();
      query = query.Where(
        produto => produto.NomeProduto.Contains(termo) || produto.Categoria.Contains(busca)
      );

    // Execução do banco SQL
    List<Produto> produtos = await query.OrderByDescending(produto => produto.Id_Produto).ToListAsync(cancellationtoken);

    // Mapeia a lista de entidades para DTOs de resposta
    return produtos.Select(produtos => MapToResponseDto(produtos)).ToList().AsReadyOnly();
  }
  
  public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationtoken = default)
  {
    var produto = await _contexto.Produtos.AsNoTracking().FirstOfDefaultAsync(produtos => produtos.Id == id, cancellationtoken);

    if (produto == null) return null;

    return MapToResponseDto(produto);
  }

  // Criar um produto novo
  public async Task<ProdutoRequestDto> CriarAsync(ProdutoRequestDto dto, CancellationToken cancellationtoken)
  {
    var produto = new Produto {
      NomeProduto = dto.Nome,
      Descricao = dto.Descricao,
      Preco = dto.PrecoBase,
      Categoria = dto.Categoria,
      Status = StatusAtivo.Ativado
    };

    _contexto.Produtos.Add(produto);
    await _contexto.SaveChangesAsync(cancellationtoken);
    return produto;
  }

  // Atualizar produto
  public async Task<bool> AltualizarAsync (int id, ProdutoRequestDto dto, CancellationToken cancellationtoken = default)

  {
    var produto = await _contexto.Produtos.FindAsync(new object[] {id}, cancellationtoken);

    if (produto == null) return false;

    produto.Nome = dto.Nome;
    produto.Descricao = dto.Descricao;
    produto.PrecoBase = dto.PrecoBase;
    produto.Categoria = dto.Categoria;
    produto.Status = dto.Status;

    await _contexto.SaveChangesAsync(cancellationtoken);
    return true;
    
  }

  // Atualizar o status do produto

  public async Task<bool> AtualizarStatusAsync (int id, CancellationToken cancellationToken = default)
  {
    var produto = await _contexto.Produtos.FindAsync(new object[] {id}, cancellationToken);

    if (produto == null)
    { return false; }

    produto.Status = !produto.Status;

    await _contexto.SaveToChangesAsync(cancellationToken);
    return true;
  }

  // Remover produto
  public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default)
  {
    // Busca o produto no banco
    var produto = await _contexto.Produtos.Findasync(new object[] { id }, cancellationToken);
    
    // e não encontrou, retorna false para a Controller tratar como NotFound
    if (produto == null) return false;

    // Remove do DbContext e salva
    _contexto.Produtos.Remove(produto);
    await _contexto.SaveChangesasync(cancellationToken);

    return true;
  }
  
  //Método auxiliar para conversão de Entidade para DTO de Resposta
  private static ProdutoResponseDto MapToResponseDto(Produto produto)
  {
    return new ProdutoResponseDto
    {
      Id = produto.Id_Produto,
      Nome = produto.NomeProduto,
      Descricao = produto.Descricao,
      PrecoBase = produto.Preco,
      Categoria = produto.Categoria,
      Status = produto.Status.ToString()
    };
    
  }

    Task<IReadOnlyCollection<ProdutoResponseDto>> IProdutoService.ObterTodasAsync(string? nome, string? categoria, StatusAtivo status, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ProdutoResponseDto?> ObterIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<ProdutoResponseDto?> IProdutoService.CriarAsync(ProdutoRequestDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ProdutoResponseDto?> AtualizarAsync(int id, ProdutoRequestDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}