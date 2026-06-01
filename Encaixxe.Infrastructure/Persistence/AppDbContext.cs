using Encaixxe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Encaixxe.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Marca> Marcas => Set<Marca>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Slug).HasMaxLength(120).IsRequired();
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marcas");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Nome).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Descricao).HasMaxLength(1000).IsRequired();
            entity.Property(p => p.Codigo).HasMaxLength(50).IsRequired();
            entity.Property(p => p.Material).HasMaxLength(100);
            entity.Property(p => p.Medidas).HasMaxLength(100);
            entity.Property(p => p.Cor).HasMaxLength(80);
            entity.Property(p => p.ImagemUrl).HasMaxLength(500);
            entity.Property(p => p.Preco).HasColumnType("decimal(10,2)");

            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Marca)
                .WithMany(m => m.Produtos)
                .HasForeignKey(p => p.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Marca>().HasData(
            new Marca { Id = 1, Nome = "Agraplast", Ativo = true }
        );

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Estantes", Slug = "estantes", Ativo = true },
            new Categoria { Id = 2, Nome = "Mesas", Slug = "mesas", Ativo = true },
            new Categoria { Id = 3, Nome = "Gaveteiros", Slug = "gaveteiros", Ativo = true },
            new Categoria { Id = 4, Nome = "Organizadores", Slug = "organizadores", Ativo = true }
        );

        modelBuilder.Entity<Produto>().HasData(
            new Produto
            {
                Id = 1,
                Nome = "Estante Organizadora",
                Descricao = "Produto Agraplast ideal para organização da casa.",
                Codigo = "AGR-001",
                Material = "Plástico",
                Medidas = "Consultar",
                Cor = "Variadas",
                Preco = null,
                ImagemUrl = null,
                Destaque = true,
                Ativo = true,
                CategoriaId = 1,
                MarcaId = 1
            },
            new Produto
            {
                Id = 2,
                Nome = "Gaveteiro Multiuso",
                Descricao = "Gaveteiro prático para quartos, escritórios e áreas de serviço.",
                Codigo = "AGR-002",
                Material = "Plástico",
                Medidas = "Consultar",
                Cor = "Variadas",
                Preco = null,
                ImagemUrl = null,
                Destaque = true,
                Ativo = true,
                CategoriaId = 3,
                MarcaId = 1
            }
        );
    }
}