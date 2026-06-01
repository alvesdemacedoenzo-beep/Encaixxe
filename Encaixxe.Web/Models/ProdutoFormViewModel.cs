using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Encaixxe.Web.Models;

public class ProdutoFormViewModel
{
    public ProdutoViewModel Produto { get; set; } = new();
    public IFormFile? ImagemArquivo { get; set; }

    public List<SelectListItem> Categorias { get; set; } = new();
    public List<SelectListItem> Marcas { get; set; } = new();
}