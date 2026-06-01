using Encaixxe.Application.Abstractions.Repositories;
using Encaixxe.Application.Filters;
using Microsoft.AspNetCore.Mvc;
using Encaixxe.Application.DTOs;

namespace Encaixxe.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProdutoFilter filter)
    {
        var result = await _produtoRepository.GetPagedAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var produto = await _produtoRepository.GetByIdAsync(id);

        if (produto is null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProdutoCreateUpdateDto dto)
    {
        var produto = await _produtoRepository.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProdutoCreateUpdateDto dto)
    {
        var atualizado = await _produtoRepository.UpdateAsync(id, dto);

        if (!atualizado)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _produtoRepository.DeleteAsync(id);

        if (!deletado)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return NoContent();
    }
}