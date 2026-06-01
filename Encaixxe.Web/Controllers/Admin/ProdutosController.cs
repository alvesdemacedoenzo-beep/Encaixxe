using Encaixxe.Web.Models;
using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Encaixxe.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Produtos")]
public class ProdutosController : Controller
{
    private readonly ApiClient _apiClient;

    public ProdutosController(ApiClient apiClient, IWebHostEnvironment environment)
    {
        _apiClient = apiClient;
        _environment = environment;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var produtos = await _apiClient.GetProdutosAdminAsync();
        return View("~/Views/Admin/Produtos/Index.cshtml", produtos);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = await BuildFormViewModelAsync();
        return View("~/Views/Admin/Produtos/Create.cshtml", viewModel);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(ProdutoFormViewModel viewModel)
    {
        if (viewModel.Produto.CategoriaId <= 0 || viewModel.Produto.MarcaId <= 0)
        {
            ModelState.AddModelError("", "Selecione uma categoria e uma marca válidas.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            var form = await BuildFormViewModelAsync(viewModel.Produto);
            return View("~/Views/Admin/Produtos/Create.cshtml", form);
        }

        var imagemUrl = await SalvarImagemAsync(viewModel.ImagemArquivo);

        if (!string.IsNullOrWhiteSpace(imagemUrl))
        {
            viewModel.Produto.ImagemUrl = imagemUrl;
        }

        var ok = await _apiClient.CreateProdutoAsync(viewModel.Produto);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao criar produto.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            var form = await BuildFormViewModelAsync(viewModel.Produto);
            return View("~/Views/Admin/Produtos/Create.cshtml", form);
        }
        TempData["Success"] = "Produto criado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var produto = await _apiClient.GetProdutoByIdAsync(id);

        if (produto is null)
            return NotFound();

        var viewModel = await BuildFormViewModelAsync(produto);
        return View("~/Views/Admin/Produtos/Edit.cshtml", viewModel);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, ProdutoFormViewModel viewModel)
    {
        if (viewModel.Produto.CategoriaId <= 0 || viewModel.Produto.MarcaId <= 0)
        {
            ModelState.AddModelError("", "Selecione uma categoria e uma marca válidas.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            var form = await BuildFormViewModelAsync(viewModel.Produto);
            return View("~/Views/Admin/Produtos/Edit.cshtml", form);
        }

        var imagemUrl = await SalvarImagemAsync(viewModel.ImagemArquivo);

        if (!string.IsNullOrWhiteSpace(imagemUrl))
        {
            viewModel.Produto.ImagemUrl = imagemUrl;
        }

        var ok = await _apiClient.UpdateProdutoAsync(id, viewModel.Produto);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao editar produto.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            var form = await BuildFormViewModelAsync(viewModel.Produto);
            return View("~/Views/Admin/Produtos/Edit.cshtml", form);
        }
        TempData["Success"] = "Produto atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _apiClient.DeleteProdutoAsync(id);
        TempData["Success"] = "Produto removido com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<ProdutoFormViewModel> BuildFormViewModelAsync(ProdutoViewModel? produto = null)
    {
        var categorias = await _apiClient.GetCategoriasAsync();
        var marcas = await _apiClient.GetMarcasAsync();

        return new ProdutoFormViewModel
        {
            Produto = produto ?? new ProdutoViewModel(),

            Categorias = categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList(),

            Marcas = marcas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nome
            }).ToList()
        };
    }
    private readonly IWebHostEnvironment _environment;

    private async Task<string?> SalvarImagemAsync(IFormFile? arquivo)
    {
        if (arquivo is null || arquivo.Length == 0)
            return null;

        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

        if (!extensoesPermitidas.Contains(extensao))
            throw new InvalidOperationException("Formato de imagem inválido.");

        var pastaUploads = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(pastaUploads))
            Directory.CreateDirectory(pastaUploads);

        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
        var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivo);

        using var stream = new FileStream(caminhoCompleto, FileMode.Create);
        await arquivo.CopyToAsync(stream);

        return $"/uploads/{nomeArquivo}";
    }
}