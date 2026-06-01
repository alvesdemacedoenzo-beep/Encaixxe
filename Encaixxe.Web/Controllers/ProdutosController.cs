using Encaixxe.Web.Models;
using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Encaixxe.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Encaixxe.Web.Controllers;

public class ProdutosController : Controller
{
    private readonly ApiClient _apiClient;

    public ProdutosController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(string? q, int? categoriaId, int? marcaId, int page = 1)
    {
        var produtos = await _apiClient.GetProdutosAsync(q, categoriaId, marcaId, page);
        var categorias = await _apiClient.GetCategoriasAsync();
        var marcas = await _apiClient.GetMarcasAsync();

        var viewModel = new ProdutoCatalogoViewModel
        {
            Produtos = produtos,
            Q = q,
            CategoriaId = categoriaId,
            MarcaId = marcaId,
            Page = page,

            Categorias = categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome,
                Selected = categoriaId == c.Id
            }).ToList(),

            Marcas = marcas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nome,
                Selected = marcaId == m.Id
            }).ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var produto = await _apiClient.GetProdutoByIdAsync(id);

        if (produto is null)
            return NotFound();

        var relacionadosResult = await _apiClient.GetProdutosAsync(
    categoriaId: produto.CategoriaId,
    marcaId: null,
    page: 1
);

        var relacionados = relacionadosResult?.Items
            .Where(p => p.Id != produto.Id)
            .Take(4)
            .ToList() ?? new List<ProdutoViewModel>();

        var viewModel = new ProdutoDetalhesViewModel
        {
            Produto = produto,
            ProdutosRelacionados = relacionados
        };

        return View(viewModel);
    }
}