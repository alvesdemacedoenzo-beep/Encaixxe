namespace Encaixxe.Application.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string? Material { get; set; }
    public string? Medidas { get; set; }
    public string? Cor { get; set; }
    public decimal? Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Destaque { get; set; }

    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;

    public int MarcaId { get; set; }
    public string MarcaNome { get; set; } = string.Empty;
}