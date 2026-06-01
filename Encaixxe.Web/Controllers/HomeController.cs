using Encaixxe.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Encaixxe.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApiClient _apiClient;

    public HomeController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _apiClient.GetProdutosDestaqueAsync();

        return View(produtos);
    }
}