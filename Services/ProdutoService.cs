using Microsoft.EntityFrameWorkCore;
using Franqueadora.API.Models;
using Franqueadora.API.DTOs;
using Franqueadora.API.Data;
using Franqueadora.API.Core.Entites;


namespace Franqueadora.API.Services;

public class ProdutoService : IProdutoService
{
  public readonly AppContext _contexto;
  
  public ProdutoService(AppContext contexto)
  {
    _contexto = contexto;
  }

  public async Task<IReadCollection<ProdutoResponseDto>> ObterTodosAsync (
    string? nome,
    string? categoria,
    bool? status,
    CancellationToken cancellationtoken = default
  )
  {
    // Inicia a consulta como IQueryable (nenhum SQL foi executado até aqui)
    var query = _contexto.Produtos.AsNoTracking().AsQueryable();
    // Aplica o filtro de Nome se foi informado (busca insensível a maiúsculas/minúsculas)
    if (!string.IsNullOfWhiteSpace(nome))
      query = query.Where(produto => produto.Nome.ToLower().Contains(nome.ToLower()));
      
    // Aplica o filtro de Categoria se foi informado
    if (!string.IsNullOfWhiteSpace(categoria))
    {
      query = query.Where(produto => produto.Categoria.ToLower() == categoria.ToLower());
    };

    // Aplica o filtro de Status se foi informado
    if (status.HasValue)
      {
        query = query.Where(produto => produto.Status == status.Value);
      }

    // Executa a consulta acumulada no banco de dados de uma só vez
    var produtos = await query.ToListAsync(cancellationtoken);

    // Mapeia a lista de entidades para DTOs de resposta
    return produtos.Select(produto => MapToResponseDto(produto)).ToList().AsReadyOnly();
  }
  
  public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationtoken = default)
  {
    var produto = await _contexto.Produtos.AsNoTracking().FirstOfDefaultAsync(produtos => produtos.Id == id, cancellationtoken);

    if (produto == null) return null;

    return MapToResponseDto(produto);
  }

  public async Task<ProdutoRequestDto> CriarAsync(ProdutoRequestDto dto, CancellationToken cancellationtoken)
  {
    var produto = new Produto {
      Nome = dto.Nome,
      Descricao = dto.Descricao,
      Preco_Base = dto.PrecoBase,
      Categoria = dto.Categoria,
      Status = dto.Status
    };

    _contexto.Produtos.Add(produto);
    await _contexto.SaveChangesAsync(cancellationtoken);
    return MapToResponseDto(produto);
  }

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

  public async Task<bool> AtualizarStatusAsync (int id, CancellationToken cancellationToken = default)
  {
    var produto = await _contexto.Produtos.FindAsync(new object[] {id}, cancellationToken);

    if (produto == null)
    { return null; }

    produto.Status = !produto.Status;

    await _contexto.SaveToChangesAsync(cancellationToken);
    return true;
  }
  
  //Método auxiliar para conversão de Entidade para DTO de Resposta
  private static ProdutoResponseDto MapToResponseDto(Produto produto)
  {
    return new ProdutoResponseDto
    {
      id = produto.Id,
      nome = produto.Nome,
      descricao = produto.Descricao,
      preco = produto.PrecoBase,
      categoria = produto.Categoria,
      status = produto.Status
    };
    
  }
}