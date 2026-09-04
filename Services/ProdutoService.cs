using Microsoft.EntityFrameWorkCore;
using Franqueadora_API.Models;
using Franqueadora_API.DTOs;
using Franqueadora_API.Data;
using Franqueadora_API.Core.Entites;


namespace Franqueadora_API.Services;

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
      query = query.Where(produto => produto.Nome.ToLower().Contains(nome.ToLower()))

    // Aplica o filtro de Categoria se foi informado
    if (!string.IsNullOfWhiteSpace(categoria))
      query = query.Where(produto => produto.Categoria.ToLower() == categoria.ToLower())

    // Aplica o filtro de Status se foi informado
    if (status.HasValue)
      query = query.Where(produto => produto.Status == status.Value)

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
    new Produto {
      Nome = dto.Nome,
      Descricao = dto.Descricao,
      Preco_Base = dto.PrecoBase,
      Categoria = dto.Categoria,
      Status = dto.Status
    };

    _contexto.Produtos.Add(Produto produto);
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
  
}