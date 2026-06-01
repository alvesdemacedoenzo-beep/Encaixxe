using Encaixxe.Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Mvc;
using Encaixxe.Application.DTOs;

namespace Encaixxe.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriasController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);

        if (categoria is null)
            return NotFound(new { message = "Categoria não encontrada." });

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaCreateUpdateDto dto)
    {
        var categoria = await _categoriaRepository.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaCreateUpdateDto dto)
    {
        var atualizado = await _categoriaRepository.UpdateAsync(id, dto);

        if (!atualizado)
            return NotFound(new { message = "Categoria não encontrada." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _categoriaRepository.DeleteAsync(id);

        if (!deletado)
            return NotFound(new { message = "Categoria não encontrada." });

        return NoContent();
    }
}