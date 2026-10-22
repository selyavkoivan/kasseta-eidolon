namespace CatalogAPI.Domain.Entities;

public class Genre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public List<Movie> Movies { get; set; } = [];
}