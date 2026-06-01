using Encaixxe.Web.Models;
using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Encaixxe.Web.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin")]
public class AdminController : Controller
{
    private readonly ApiClient _apiClient;

    public AdminController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var produtos = await _apiClient.GetProdutosAdminAsync();
        var categorias = await _apiClient.GetCategoriasAsync();
        var marcas = await _apiClient.GetMarcasAsync();

        var viewModel = new AdminDashboardViewModel
        {
            TotalProdutos = produtos?.TotalItems ?? 0,
            TotalCategorias = categorias.Count,
            TotalMarcas = marcas.Count,
            TotalDestaques = produtos?.Items.Count(p => p.Destaque) ?? 0
        };

        return View("~/Views/Admin/Index.cshtml", viewModel);
    }
}