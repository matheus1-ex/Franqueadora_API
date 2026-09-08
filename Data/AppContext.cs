using System.Data.Common;
using Franqueada.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Franqueada.API.Data;

public sealed class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options) {}

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

            entidade.Property(unidade => unidade.Nome_Unidade).HasMaxLength(100).IsRequired();

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
    }
    
} 