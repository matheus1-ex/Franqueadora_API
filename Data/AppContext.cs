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
    }




    
} 