using Encaixxe.Application.Abstractions.Repositories;
using Encaixxe.Application.DTOs;
using Encaixxe.Application.Filters;
using Encaixxe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Encaixxe.Domain.Entities;

namespace Encaixxe.Infrastructure.Repositories;

public class EfProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public EfProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProdutoDto>> GetPagedAsync(ProdutoFilter filter)
    {
        var query = _context.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Marca)
            .Where(p => p.Ativo);

        if (!string.IsNullOrWhiteSpace(filter.Q))
        {
            query = query.Where(p =>
                p.Nome.Contains(filter.Q) ||
                p.Descricao.Contains(filter.Q) ||
                p.Codigo.Contains(filter.Q));
        }

        if (filter.CategoriaId.HasValue)
        {
            query = query.Where(p => p.CategoriaId == filter.CategoriaId.Value);
        }

        if (filter.MarcaId.HasValue)
        {
            query = query.Where(p => p.MarcaId == filter.MarcaId.Value);
        }

        if (filter.Destaque.HasValue)
        {
            query = query.Where(p => p.Destaque == filter.Destaque.Value);
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.Destaque)
            .ThenBy(p => p.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Codigo = p.Codigo,
                Material = p.Material,
                Medidas = p.Medidas,
                Cor = p.Cor,
                Preco = p.Preco,
                ImagemUrl = p.ImagemUrl,
                Destaque = p.Destaque,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria.Nome,
                MarcaId = p.MarcaId,
                MarcaNome = p.Marca.Nome
            })
            .ToListAsync();

        return new PagedResult<ProdutoDto>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalItems = totalItems
        };
    }

    public async Task<ProdutoDto?> GetByIdAsync(int id)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Marca)
            .Where(p => p.Ativo && p.Id == id)
            .Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Codigo = p.Codigo,
                Material = p.Material,
                Medidas = p.Medidas,
                Cor = p.Cor,
                Preco = p.Preco,
                ImagemUrl = p.ImagemUrl,
                Destaque = p.Destaque,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria.Nome,
                MarcaId = p.MarcaId,
                MarcaNome = p.Marca.Nome
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProdutoDto> CreateAsync(ProdutoCreateUpdateDto dto)
    {
        var produto = new Encaixxe.Domain.Entities.Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Codigo = dto.Codigo,
            Material = dto.Material,
            Medidas = dto.Medidas,
            Cor = dto.Cor,
            Preco = dto.Preco,
            ImagemUrl = dto.ImagemUrl,
            Destaque = dto.Destaque,
            Ativo = dto.Ativo,
            CategoriaId = dto.CategoriaId,
            MarcaId = dto.MarcaId
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var produtoCriado = await GetByIdAsync(produto.Id);

        return produtoCriado!;
    }

    public async Task<bool> UpdateAsync(int id, ProdutoCreateUpdateDto dto)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
            return false;

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Codigo = dto.Codigo;
        produto.Material = dto.Material;
        produto.Medidas = dto.Medidas;
        produto.Cor = dto.Cor;
        produto.Preco = dto.Preco;
        produto.ImagemUrl = dto.ImagemUrl;
        produto.Destaque = dto.Destaque;
        produto.Ativo = dto.Ativo;
        produto.CategoriaId = dto.CategoriaId;
        produto.MarcaId = dto.MarcaId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
            return false;

        produto.Ativo = false;

        await _context.SaveChangesAsync();

        return true;
    }
}