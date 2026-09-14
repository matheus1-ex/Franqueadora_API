
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

    public DbSet<Chamado> Chamados { get; set; }


    
  
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

                entidade.Property(usuario => usuario.Nome).IsRequired().HasMaxLength(40);

                entidade.Property(usuario => usuario.Email).HasMaxLength(250).IsRequired();

                entidade.Property(usuario => usuario.SenhaHash).HasMaxLength(250).IsRequired();

                entidade.Property(usuario => usuario.Status).HasConversion<string>().HasMaxLength(30).IsRequired();

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

                entidade.Property(usuario => usuario.Status).HasConversion<string>().IsRequired().HasMaxLength(45);
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

            entidade.Property(produto => produto.Status).IsRequired().HasConversion<string>().HasDefaultValue(StatusAtivo.Desativado).HasSentinel((StatusAtivo)(-1));

            // Chave Estrangeira Opcional para Fornecedor
            entidade.Property(p => p.FornecedorId).IsRequired();

            // Relacionamento com Fornecedor
            entidade.HasOne(produto => produto.Fornecedor)
                .WithMany(fornecedor => fornecedor.Produtos)
                .HasForeignKey(produto => produto.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);
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

        // ====================
        // Configuração de Royalty
        // ====================
        modelBuilder.Entity<ConfiguracaoRoyalty>(entidade =>
        {
            entidade.ToTable("ConfiguracoesRoyalty");

            entidade.HasKey(config => config.Id);

            entidade.Property(config => config.PercentualRoyalty)
                .IsRequired()
                .HasPrecision(5, 2); // Permite até 999.99% (ex: 5.00 para 5%)

            // Relacionamento com Unidade (1 Unidade tem 1 Configuração de Royalty)
            entidade.HasOne(config => config.Unidade)
                .WithMany()
                .HasForeignKey(config => config.UnidadeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Garante que cada unidade só tenha UMA configuração de royalty cadastrada
            entidade.HasIndex(config => config.UnidadeId)
                .IsUnique();
        });

        // ====================
        // Lançamento de Royalty
        // ====================
        modelBuilder.Entity<LancamentoRoyalty>(entidade =>
        {
            entidade.ToTable("LancamentosRoyalty");

            entidade.HasKey(lancamento => lancamento.Id);

            entidade.Property(lancamento => lancamento.MesReferencia)
                .IsRequired();

            entidade.Property(lancamento => lancamento.AnoReferencia)
                .IsRequired();

            entidade.Property(lancamento => lancamento.FaturamentoPeriodo)
                .IsRequired()
                .HasPrecision(18, 2); // Precisão monetária (Ex: R$ 150.000,50)

            entidade.Property(l => l.PercentualAplicado)
                .IsRequired()
                .HasPrecision(5, 2);  // Precisão percentual (Ex: 5.00%)

            // Armazena o Enum StatusPagamento ("Pendente", "Pago", "Atrasado") como string no banco
            entidade.Property(lancamento => lancamento.Status).IsRequired().HasConversion<string>();

            entidade.Property(lancamento => lancamento.DataPagamento).IsRequired(false);

            // Propriedade calculada em memória (não cria coluna no banco de dados)
            entidade.Ignore(lancamento => lancamento.ValorDevido);

            // Relacionamento com Unidade
            entidade.HasOne(lancamento => lancamento.Unidade)
                .WithMany()
                .HasForeignKey(lancamento => lancamento.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Evita lançamentos duplicados para a mesma unidade no mesmo mês/ano
            entidade.HasIndex(lancamento => new { lancamento.UnidadeId, lancamento.MesReferencia, lancamento.AnoReferencia })
                .IsUnique();
        });

        // ====================
        // Fornecedor
        // ====================
        modelBuilder.Entity<Fornecedor>(entidade =>
        {
            entidade.ToTable("Fornecedores");

            entidade.HasKey(fornecedor => fornecedor.Id);

            // Campos de Texto Obrigatórios e Limites de Tamanho
            entidade.Property(fornecedor => fornecedor.NomeRazaoSocial)
                .IsRequired()
                .HasMaxLength(150);

            entidade.Property(fornecedor => fornecedor.Cnpj)
                .IsRequired()
                .HasMaxLength(18); // Formato com máscara: "00.000.000/0000-00"

            entidade.Property(fornecedor => fornecedor.Telefone)
                .HasMaxLength(20);

            entidade.Property(fornecedor => fornecedor.Email)
                .HasMaxLength(100);

            // Armazena o Enum StatusAtivo ("Ativado", "Desativado") como string no banco
            entidade.Property(fornecedor => fornecedor.Status)
                .IsRequired()
                .HasConversion<string>();

            // Garante que não existirão dois fornecedores com o mesmo CNPJ
            entidade.HasIndex(fornecedor => fornecedor.Cnpj).IsUnique();

            // Relacionamento 1 para N com Produtos (Um fornecedor fornece N produtos)
            entidade.HasMany(fornecedor => fornecedor.Produtos)
                .WithOne(produto => produto.Fornecedor)
                .HasForeignKey(produto => produto.FornecedorId)
                .OnDelete(DeleteBehavior.SetNull); // Se o fornecedor for excluído, o produto apenas fica com FornecedorId nulo
        });


        // ====================
        // Chamado
        // ====================
        // Mapeamento da entidade Chamado
            modelBuilder.Entity<Chamado>(entity =>
            {
                entity.ToTable("Chamados");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Titulo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.Descricao)
                    .IsRequired();

                entity.Property(c => c.Categoria)
                    .IsRequired();

                entity.Property(c => c.Prioridade)
                    .IsRequired();

                entity.Property(c => c.Status)
                    .IsRequired();

                entity.Property(c => c.DataAbertura)
                    .IsRequired();

                // Relacionamento: Um Chamado pertence a uma Unidade (1:N)
                entity.HasOne(c => c.Unidade)
                    .WithMany() // ou .WithMany(u => u.Chamados) se houver a coleção na classe Unidade
                    .HasForeignKey(c => c.UnidadeId)
                    .OnDelete(DeleteBehavior.Restrict); // Evita deletar a unidade em cascata caso haja chamados vinculados
            });
    }
} 