using Encaixxe.Application.Abstractions.Repositories;
using Encaixxe.Application.DTOs;
using Encaixxe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Encaixxe.Infrastructure.Repositories;

public class EfMarcaRepository : IMarcaRepository
{
    private readonly AppDbContext _context;

    public EfMarcaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MarcaDto>> GetAllAsync()
    {
        return await _context.Marcas
            .AsNoTracking()
            .Where(m => m.Ativo)
            .OrderBy(m => m.Nome)
            .Select(m => new MarcaDto
            {
                Id = m.Id,
                Nome = m.Nome
            })
            .ToListAsync();
    }
    public async Task<MarcaDto?> GetByIdAsync(int id)
    {
        return await _context.Marcas
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new MarcaDto
            {
                Id = m.Id,
                Nome = m.Nome
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MarcaDto> CreateAsync(MarcaCreateUpdateDto dto)
    {
        var marca = new Encaixxe.Domain.Entities.Marca
        {
            Nome = dto.Nome,
            Ativo = dto.Ativo
        };

        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();

        return new MarcaDto
        {
            Id = marca.Id,
            Nome = marca.Nome
        };
    }

    public async Task<bool> UpdateAsync(int id, MarcaCreateUpdateDto dto)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca is null)
            return false;

        marca.Nome = dto.Nome;
        marca.Ativo = dto.Ativo;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);

        if (marca is null)
            return false;

        marca.Ativo = false;

        await _context.SaveChangesAsync();

        return true;
    }
}