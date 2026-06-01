namespace Encaixxe.Application.Filters;

public class ProdutoFilter
{
    public string? Q { get; set; }
    public int? CategoriaId { get; set; }
    public int? MarcaId { get; set; }
    public bool? Destaque { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}