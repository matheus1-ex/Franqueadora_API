using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Franqueada.API.Models;
using Franqueada.API.DTOs;
using Franqueada.API.Data;


namespace Franqueada.API.Services;

public sealed class ProdutoService : IProdutoService
{
  public readonly AppDbContext _contexto;
  
  public ProdutoService(AppDbContext contexto)
  {
    _contexto = contexto;
  }

/// <summary>
/// Converte um objeto do tipo Produto em um ProdutoResponseDto.
/// </summary>
/// <param name="produto"></param>
/// <returns></returns>
  private static ProdutoResponseDto MapToDto(Produto produto)
  {
      return new ProdutoResponseDto
      {
          Id = produto.Id_Produto,
          Nome = produto.NomeProduto,
          Descricao = produto.Descricao ?? string.Empty,
          Categoria = produto.Categoria,
          PrecoBase = produto.Preco,
          QuantidadeEstoque = produto.QuantidadeEstoque,
          FornecedorID = produto.FornecedorId,
          Status = produto.Status
      };
  }
    public async Task<bool> AlternarStatusAsync(int id, StatusAtivo status, CancellationToken cancellationToken)
    {
        var produto = await _contexto.Produtos.FindAsync(new object[] {id}, cancellationToken);

        if (produto == null) 
        {
          return false;
        }

        produto.Status = status;

        await _contexto.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ProdutoResponseDto?> AtualizarAsync(int id, ProdutoRequestDto dto, CancellationToken cancellationToken)
    {
        var produto = await _contexto.Produtos.FindAsync(new object[] {id}, cancellationToken);

        if (produto == null) return null;

        produto.NomeProduto = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.PrecoBase;
        produto.Categoria = dto.Categoria;
        produto.Status = StatusAtivo.Ativado;

        await _contexto.SaveChangesAsync(cancellationToken);
        return MapToDto(produto);
    }

    public async Task<List<ProdutoResponseDto>> CriarAsync(List<ProdutoRequestDto> dto, CancellationToken cancellationToken)
    {
        var produtos = dto.Select(dto => new Produto
        {
            NomeProduto = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.PrecoBase,
            Categoria = dto.Categoria,
            Status = StatusAtivo.Ativado,
            QuantidadeEstoque = dto.QuantidadeEstoque,
            FornecedorId = dto.FornecedorID
        }).ToList();

        Console.WriteLine($"[DB DEBUG] Banco conectado: {_contexto.Database.GetDbConnection().ConnectionString}");

        // Adiciona todos no banco e salva
        _contexto.Produtos.AddRange(produtos);
        await _contexto.SaveChangesAsync(cancellationToken);

        // Mapeia as entidades salvas (com os IDs gerados) de volta para DTO
        return produtos.Select(p => MapToDto(p)).ToList();
    }

    public async Task<ProdutoResponseDto?> ObterIdAsync(int id, CancellationToken cancellationToken)
    {
        var produto = await _contexto.Produtos.AsNoTracking().FirstOrDefaultAsync(produtos => produtos.Id_Produto == id, cancellationToken);
        if (produto == null) return null;

        return MapToDto(produto);
    }

    public async Task<IReadOnlyCollection<ProdutoResponseDto>> ObterTodasAsync(
      string? nome, 
      string? categoria,
      string? busca, 
      StatusAtivo? status, 
      CancellationToken cancellationToken)
    {
      // Inicia a consulta como IQueryable 
      IQueryable<Produto> query = _contexto.Produtos.AsNoTracking();

      // Aplica o filtro de Nome se foi informado 
      if (!string.IsNullOrWhiteSpace(nome))
        query = query.Where(produto => produto.NomeProduto.Contains(nome));
        
      // Aplica o filtro de Categoria se foi informado
      if (!string.IsNullOrWhiteSpace(categoria))
      {
        query = query.Where(produto => produto.Categoria == categoria);
      };

      // Aplica o filtro de Status se foi informado
      if (status.HasValue)
        {
          query = query.Where(produto => produto.Status == status);
        }

      // Busca pelo Nome ou Categoria
      if (!string.IsNullOrWhiteSpace(busca))
    {
      string termo = busca.Trim();
        query = query.Where(
          produto => produto.NomeProduto != null || produto.Categoria != null
        );
    }

      // Execução do banco SQL
      List<Produto> produtos = await query.OrderByDescending(produto => produto.Id_Produto).ToListAsync(cancellationToken);

      // Mapeia a lista de entidades para DTOs de resposta
      return produtos.Select(MapToDto).ToList().AsReadOnly();
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken)
    {
        // Busca o produto no banco
        var produto = await _contexto.Produtos.FindAsync(new object[] { id }, cancellationToken);
        
        // e não encontrou, retorna false para a Controller tratar como NotFound
        if (produto == null) return false;

        // Remove do DbContext e salva
        _contexto.Produtos.Remove(produto);
        await _contexto.SaveChangesAsync(cancellationToken);

        return true;
    }

}