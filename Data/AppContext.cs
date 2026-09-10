
using Franqueada.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Franqueada.API.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    /// <summary>
    /// Tabela de Usuários
    /// </summary>
    public DbSet<Usuario> Usuarios  => Set<Usuario>(); 

    /// <summary>
    /// Tabela de Perfil
    /// </summary>
    public DbSet<Perfil> Perfis {get; set;}

    /// <summary>
    /// Tabela de Unidades
    /// </summary>
    public DbSet<Unidade> Unidades {get; set;}

    /// <summary>
    /// Tabela de Franquias
    /// </summary>
    public DbSet<Franquia> Franquias {get; set;}

    /// <summary>
    /// Tabela de Franqueadoras
    /// </summary>
    public DbSet<Franqueadores> Franqueadoras {get; set;}

    /// <summary>
    /// Tabela de Produtos
    /// </summary>
    public DbSet<Produto> Produtos {get; set;}

    /// <summary>
    /// Tabela de Estoque
    /// </summary>
    public DbSet<Estoque> Estoques { get; set; }

    /// <summary>
    /// Movimento de estoque
    /// </summary>
    public DbSet<MovimentoEstoque> MovimentacoesEstoque { get; set; }

    /// <summary>
    /// Tabela de venda
    /// </summary>
    public DbSet<Venda> Vendas { get; set; }

    /// <summary>
    /// Tabela de Itens (produtos) vendidos
    /// </summary>
    public DbSet<ItemVenda> ItensVenda { get; set; }

    public DbSet<ConfiguracaoRoyalty> ConfiguracoesRoyalty { get; set; }

    public DbSet<LancamentoRoyalty> LancamentosRoyalty { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // ==================================
        // Usuários
        // ==================================
        modelBuilder.Entity<Usuario>(
            entidade =>
            {
                entidade.ToTable("Usuários");

                entidade.HasKey(usuario => usuario.Id);

                entidade.Property(usuario => usuario.Email).HasMaxLength(250).IsRequired();

                entidade.Property(usuario => usuario.SenhaHash).HasMaxLength(250).IsRequired();
            }
        );

        // ==================================
        // Perfis
        // ==================================
        modelBuilder.Entity<Perfil>(
            entidade =>
            {
                entidade.ToTable("Perfis");

                entidade.HasKey(usuario => usuario.Id);

                entidade.Property(usuario => usuario.Nome).HasMaxLength(50).IsRequired();
            }
        );
        // ==========================
        // Unidades
        // ==========================
        modelBuilder.Entity<Unidade>(
          entidade =>
          {
            entidade.ToTable("Unidades");
            entidade.HasKey(unidade => unidade.Id_Und);

            entidade.Property(unidade => unidade.Nome).HasMaxLength(100).IsRequired();

            entidade.Property(unidade => unidade.Cod_Identificador);

            entidade.Property(unidade => unidade.Endereco).HasMaxLength(250).IsRequired();

          }
        );
        // ========================
        // Franquias
        // ========================
        modelBuilder.Entity<Franquia>(
          entidade =>
          {
            entidade.ToTable("Franquias");
            
            entidade.HasKey(franquia => franquia.Id_Franquia);
            
            entidade.Property(franquia => franquia.Nome_Marca).IsRequired().HasMaxLength(200);

            entidade.Property(franquia => franquia.Cnpj).HasMaxLength(20).IsRequired();

            entidade.HasIndex(franquia => franquia.Status);
          }
        );
        // ====================
        // Franqueadoras
        // ====================
        modelBuilder.Entity<Franqueadores>(
          entidade =>
          {
            entidade.ToTable("Franqueadoras");
            
            entidade.HasKey(franqueadora => franqueadora.Id_Franqueadora);

            entidade.Property(franqueadora => franqueadora.Razao_Social).IsRequired().HasMaxLength(200);

            entidade.Property(franqueadora => franqueadora.Cnpj).HasMaxLength(20).IsRequired();

            entidade.HasIndex(franqueadora => franqueadora.Status);
          }
        );

        // ====================
        // Produtos
        // ====================
        modelBuilder.Entity<Produto>(
          entidade =>
          {
            entidade.ToTable("Produtos");
            
            entidade.HasKey(produto => produto.Id_Produto);

            entidade.Property(produto =>  produto.NomeProduto).IsRequired().HasMaxLength(100);

            entidade.Property(produto => produto.Preco).IsRequired().HasPrecision(18, 2);

            entidade.Property(produto => produto.Categoria).IsRequired().HasMaxLength(50);

            entidade.Property(produto => produto.Status).IsRequired().HasDefaultValue(false);
          }
        );

        // ====================
        // Estoque
        // ====================
        modelBuilder.Entity<Estoque>(
          entidade =>
          {
            entidade.ToTable("Estoques");
            
            entidade.HasKey(estoque => estoque.Id);

            entidade.Property(estoque =>  estoque.Quantidade).IsRequired().HasMaxLength(100);

            entidade.Property(estoque => estoque.EstoqueMinimo).IsRequired().HasDefaultValue(5);

            entidade.HasOne(estoque => estoque.Produto).WithMany().HasForeignKey(estoque => estoque.ProdutoId).OnDelete(DeleteBehavior.Restrict);

            entidade.HasOne(estoque => estoque.Unidade).WithMany().HasForeignKey(estoque => estoque.UnidadeId).OnDelete(DeleteBehavior.Restrict);

            entidade.HasIndex(estoque => new { estoque.ProdutoId, estoque.UnidadeId }).IsUnique();
          }
        );

        // ====================
        // Vendas
        // ====================
        modelBuilder.Entity<Venda>(
          entidade =>
          {
            entidade.HasKey(venda => venda.Id);

            entidade.Property(venda => venda.DataVenda).IsRequired();

            entidade.Property(venda => venda.ValorTotal).IsRequired().HasPrecision(18, 2); 

            entidade.HasOne(venda => venda.Unidade).WithMany().HasForeignKey(venda => venda.UnidadeId).OnDelete(DeleteBehavior.Restrict);

            entidade.HasMany(venda => venda.Itens).WithOne(itenVenda=> itenVenda.Venda).HasForeignKey(itenVenda => itenVenda.VendaId).OnDelete(DeleteBehavior.Cascade); 
            }
        );

        // ====================
        // Movimentação de Estoque
        // ====================
        modelBuilder.Entity<MovimentoEstoque>(entidade =>
        {
            entidade.ToTable("Movimentacoes");

            entidade.HasKey(movimento => movimento.Id);

            entidade.Property(movimento => movimento.Quantidade).IsRequired();

            entidade.Property(movimento => movimento.Tipo).IsRequired().HasConversion<string>(); // Salva "Entrada" / "Saida" como texto no banco

            entidade.Property(movimento => movimento.DataMovimentacao).IsRequired();

            entidade.Property(movimento => movimento.Observacao).HasMaxLength(250);

            // Chave Estrangeira para Estoque
            entidade.HasOne(movimento => movimento.Estoque).WithMany().HasForeignKey(movimento => movimento.EstoqueId).OnDelete(DeleteBehavior.Cascade);
        });

        // ====================
        // Itens da Venda
        // ====================
        modelBuilder.Entity<ItemVenda>(entidade =>
        {
            entidade.ToTable("ItensVenda");

            entidade.HasKey(itemVenda => itemVenda.Id);

            entidade.Property(itemVenda => itemVenda.Quantidade)
                .IsRequired();

            entidade.Property(itemVenda => itemVenda.PrecoUnitario)
                .IsRequired()
                .HasPrecision(18, 2);

            // Ignora a propriedade calculada Subtotal (não cria coluna no banco)
            entidade.Ignore(itemVenda => itemVenda.Subtotal);

            // Relacionamento com Produto
            entidade.HasOne(itemVenda => itemVenda.Produto)
                .WithMany()
                .HasForeignKey(itemVenda => itemVenda.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

    }
} 