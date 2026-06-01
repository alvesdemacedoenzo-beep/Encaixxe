using Encaixxe.Web.Models;
using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Encaixxe.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Marcas")]
public class MarcasController : Controller
{
    private readonly ApiClient _apiClient;

    public MarcasController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var marcas = await _apiClient.GetMarcasAsync();
        return View("~/Views/Admin/Marcas/Index.cshtml", marcas);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Marcas/Create.cshtml", new MarcaViewModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(MarcaViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Nome))
        {
            ModelState.AddModelError("", "O nome da marca é obrigatório.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Marcas/Create.cshtml", model);
        }

        var ok = await _apiClient.CreateMarcaAsync(model);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao criar marca.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Marcas/Create.cshtml", model);
        }
        TempData["Success"] = "Marca criada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var marca = await _apiClient.GetMarcaByIdAsync(id);

        if (marca is null)
            return NotFound();

        return View("~/Views/Admin/Marcas/Edit.cshtml", marca);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, MarcaViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Nome))
        {
            ModelState.AddModelError("", "O nome da marca é obrigatório.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Marcas/Edit.cshtml", model);
        }

        var ok = await _apiClient.UpdateMarcaAsync(id, model);

        if (!ok)
        {
            ModelState.AddModelError("", "Erro ao editar marca.");
            TempData["Error"] = "Não foi possível concluir a operação.";
            return View("~/Views/Admin/Marcas/Edit.cshtml", model);
        }
        TempData["Success"] = "Marca atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _apiClient.DeleteMarcaAsync(id);
        TempData["Success"] = "Marca removida com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}