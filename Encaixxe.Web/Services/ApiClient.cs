using Encaixxe.Web.Models;
using System.Net.Http.Json;

namespace Encaixxe.Web.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResultViewModel<ProdutoViewModel>?> GetProdutosAsync(
    string? q = null,
    int? categoriaId = null,
    int? marcaId = null,
    int page = 1)
    {
        var url = $"api/produtos?page={page}&pageSize=12";

        if (!string.IsNullOrWhiteSpace(q))
            url += $"&q={Uri.EscapeDataString(q)}";

        if (categoriaId.HasValue)
            url += $"&categoriaId={categoriaId.Value}";

        if (marcaId.HasValue)
            url += $"&marcaId={marcaId.Value}";

        return await _httpClient.GetFromJsonAsync<PagedResultViewModel<ProdutoViewModel>>(url);
    }

    public async Task<ProdutoViewModel?> GetProdutoByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProdutoViewModel>($"api/produtos/{id}");
    }

    public async Task<bool> CreateProdutoAsync(ProdutoViewModel produto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/produtos", produto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProdutoAsync(int id, ProdutoViewModel produto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/produtos/{id}", produto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProdutoAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/produtos/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoriaViewModel>> GetCategoriasAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CategoriaViewModel>>("api/categorias")
               ?? new List<CategoriaViewModel>();
    }

    public async Task<List<MarcaViewModel>> GetMarcasAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MarcaViewModel>>("api/marcas")
               ?? new List<MarcaViewModel>();
    }

    public async Task<CategoriaViewModel?> GetCategoriaByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CategoriaViewModel>($"api/categorias/{id}");
    }

    public async Task<bool> CreateCategoriaAsync(CategoriaViewModel categoria)
    {
        var response = await _httpClient.PostAsJsonAsync("api/categorias", categoria);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoriaAsync(int id, CategoriaViewModel categoria)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/categorias/{id}", categoria);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoriaAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/categorias/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<MarcaViewModel?> GetMarcaByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<MarcaViewModel>($"api/marcas/{id}");
    }

    public async Task<bool> CreateMarcaAsync(MarcaViewModel marca)
    {
        var response = await _httpClient.PostAsJsonAsync("api/marcas", marca);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateMarcaAsync(int id, MarcaViewModel marca)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/marcas/{id}", marca);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMarcaAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/marcas/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<PagedResultViewModel<ProdutoViewModel>?> GetProdutosDestaqueAsync()
    {
        return await _httpClient.GetFromJsonAsync<PagedResultViewModel<ProdutoViewModel>>(
            "api/produtos?page=1&pageSize=8&destaque=true");
    }
    public async Task<PagedResultViewModel<ProdutoViewModel>?> GetProdutosAdminAsync()
    {
        return await _httpClient.GetFromJsonAsync<PagedResultViewModel<ProdutoViewModel>>(
            "api/produtos?page=1&pageSize=1000");
    }

}