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
    public DbSet<Usuario> Usuarios {get; set;} 

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
    public DbSet<Franqueadora> Franqueadoras {get; set;}
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // ==================================
        // Usuários
        // ==================================
        modelBuilder.Entity<Usuarios>(
            entidade =>
            {
                entidade.ToTable("Usuários");

                entidade.HasKey(usuario => usuario.Id);

                entidade.Property(usuario => usuario.Nome).HasMaxLenght(50).IsRequired();

                entidade.Property(usuario => usuario.Email).HasMaxLenght(250).IsRequired();

                entidade.Property(usuario => usuario.Senha).HasMaxLenght(250).IsRequired();
            }
        );

        // ==================================
        // Perfis
        // ==================================
        modelBuilder.Entity<Perfis>(
            entidade =>
            {
                entidade.ToTable("Perfis");

                entidade.HasKey(usuario => usuario.Id);

                entidade.Property(usuario => usuario.Nome).HasMaxLenght(50).IsRequired();
            }
        );
        // ==========================
        // Unidades
        // ==========================
        modelBuilder.Entity<Unidades>(
          entidade =>
          {
            entidade.ToTable("Unidades");
            entidade.HasKey(unidade => unidade.Id);
            entidade.Property(unidade => unidade.Nome_Unidade).HasMaxLenght(100).IsRequired();
            entidade.Property(unidade => unidade.Cod_Identificador).IsUnique();

            entidade.Property(unidade => unidade.Endereco).HasMax(500).IsRequired();

          }
        );
        // ========================
        // Franquias
        // ========================
        modelBuilder.Entity<Franquias>(
          entidade =>
          {
            entidade.ToTable("Franquias");
            
            entidade.HasKey(franquia => franquia.Id_Franquia);
            
            entidade.Property(franquia => franquia.Nome_Marca).IsRequired().HasMax(200);

            entidade.Property(franquia => franquia.Cnpj).HasMax(20).IsRequired();

            entidade.Property(franquia => franquia.StatusAtivo);
          }
        );
        // ====================
        // Franqueadoras
        // ====================
    }



    
} 