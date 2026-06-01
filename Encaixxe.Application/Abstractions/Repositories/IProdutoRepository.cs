using Encaixxe.Application.DTOs;
using Encaixxe.Application.Filters;

namespace Encaixxe.Application.Abstractions.Repositories;

public interface IProdutoRepository
{
    Task<PagedResult<ProdutoDto>> GetPagedAsync(ProdutoFilter filter);
    Task<ProdutoDto?> GetByIdAsync(int id);

    Task<ProdutoDto> CreateAsync(ProdutoCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, ProdutoCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}