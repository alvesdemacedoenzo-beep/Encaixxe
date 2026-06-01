using Encaixxe.Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Mvc;
using Encaixxe.Application.DTOs;

namespace Encaixxe.Api.Controllers;

[ApiController]
[Route("api/marcas")]
public class MarcasController : ControllerBase
{
    private readonly IMarcaRepository _marcaRepository;

    public MarcasController(IMarcaRepository marcaRepository)
    {
        _marcaRepository = marcaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var marcas = await _marcaRepository.GetAllAsync();
        return Ok(marcas);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var marca = await _marcaRepository.GetByIdAsync(id);

        if (marca is null)
            return NotFound(new { message = "Marca não encontrada." });

        return Ok(marca);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MarcaCreateUpdateDto dto)
    {
        var marca = await _marcaRepository.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = marca.Id }, marca);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MarcaCreateUpdateDto dto)
    {
        var atualizado = await _marcaRepository.UpdateAsync(id, dto);

        if (!atualizado)
            return NotFound(new { message = "Marca não encontrada." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _marcaRepository.DeleteAsync(id);

        if (!deletado)
            return NotFound(new { message = "Marca não encontrada." });

        return NoContent();
    }
}