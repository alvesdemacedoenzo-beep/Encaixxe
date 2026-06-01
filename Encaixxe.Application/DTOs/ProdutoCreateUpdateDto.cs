namespace Encaixxe.Application.DTOs;

public class ProdutoCreateUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string? Material { get; set; }
    public string? Medidas { get; set; }
    public string? Cor { get; set; }
    public decimal? Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Destaque { get; set; }
    public bool Ativo { get; set; } = true;

    public int CategoriaId { get; set; }
    public int MarcaId { get; set; }
}