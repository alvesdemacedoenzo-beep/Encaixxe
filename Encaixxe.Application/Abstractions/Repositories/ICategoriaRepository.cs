using Encaixxe.Application.DTOs;

namespace Encaixxe.Application.Abstractions.Repositories;

public interface ICategoriaRepository
{
    Task<List<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> GetByIdAsync(int id);
    Task<CategoriaDto> CreateAsync(CategoriaCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, CategoriaCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}