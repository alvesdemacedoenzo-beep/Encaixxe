namespace Encaixxe.Application.DTOs;

public class CategoriaCreateUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}