using Microsoft.AspNetCore.Mvc.Rendering;

namespace Encaixxe.Web.Models;

public class ProdutoCatalogoViewModel
{
    public PagedResultViewModel<ProdutoViewModel>? Produtos { get; set; }

    public string? Q { get; set; }
    public int? CategoriaId { get; set; }
    public int? MarcaId { get; set; }
    public int Page { get; set; } = 1;

    public List<SelectListItem> Categorias { get; set; } = new();
    public List<SelectListItem> Marcas { get; set; } = new();
}