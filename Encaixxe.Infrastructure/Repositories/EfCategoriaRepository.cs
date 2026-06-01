using Encaixxe.Application.Abstractions.Repositories;
using Encaixxe.Application.DTOs;
using Encaixxe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Encaixxe.Infrastructure.Repositories;

public class EfCategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public EfCategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> GetAllAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Slug = c.Slug
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Slug = c.Slug
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaDto> CreateAsync(CategoriaCreateUpdateDto dto)
    {
        var categoria = new Encaixxe.Domain.Entities.Categoria
        {
            Nome = dto.Nome,
            Slug = dto.Slug,
            Ativo = dto.Ativo
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return new CategoriaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Slug = categoria.Slug
        };
    }

    public async Task<bool> UpdateAsync(int id, CategoriaCreateUpdateDto dto)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria is null)
            return false;

        categoria.Nome = dto.Nome;
        categoria.Slug = dto.Slug;
        categoria.Ativo = dto.Ativo;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria is null)
            return false;

        categoria.Ativo = false;

        await _context.SaveChangesAsync();

        return true;
    }
}