using Encaixxe.Application.DTOs;

namespace Encaixxe.Application.Abstractions.Repositories;

public interface IMarcaRepository
{
    Task<List<MarcaDto>> GetAllAsync();
    Task<MarcaDto?> GetByIdAsync(int id);
    Task<MarcaDto> CreateAsync(MarcaCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, MarcaCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}