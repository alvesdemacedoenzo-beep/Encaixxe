namespace Encaixxe.Web.Models;

public class ProdutoDetalhesViewModel
{
    public ProdutoViewModel Produto { get; set; } = new();

    public List<ProdutoViewModel> ProdutosRelacionados { get; set; } = new();
}