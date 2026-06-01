using Encaixxe.Web.Models;
using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Encaixxe.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Categorias")]
public class CategoriasController : Controller
{
    private readonly ApiClient _apiClient;

    public CategoriasController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var categorias = await _apiClient.GetCategoriasAsync();
        return View("~/Views/Admin/Categorias/Index.cshtml", categorias);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Categorias/Create.cshtml", new CategoriaViewModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CategoriaViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Nome))
        {
            ModelState.AddModelError("", "O nome da categoria é obrigatório.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Categorias/Create.cshtml", model);
        }

        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = model.Nome
                .ToLower()
                .Replace(" ", "-");
        }

        var ok = await _apiClient.CreateCategoriaAsync(model);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao criar categoria.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Categorias/Create.cshtml", model);
        }
        TempData["Success"] = "Categoria criada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var categoria = await _apiClient.GetCategoriaByIdAsync(id);

        if (categoria is null)
            return NotFound();

        return View("~/Views/Admin/Categorias/Edit.cshtml", categoria);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CategoriaViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Nome))
        {
            ModelState.AddModelError("", "O nome da categoria é obrigatório.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Categorias/Edit.cshtml", model);
        }

        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = model.Nome
                .ToLower()
                .Replace(" ", "-");
        }

        var ok = await _apiClient.UpdateCategoriaAsync(id, model);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao editar categoria.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Categorias/Edit.cshtml", model);
        }
        TempData["Success"] = "Categoria atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _apiClient.DeleteCategoriaAsync(id);
        TempData["Success"] = "Categoria removida com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}