namespace Encaixxe.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}