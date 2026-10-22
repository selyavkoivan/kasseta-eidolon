namespace CatalogAPI.Domain.Entities;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public List<Genre> Genres { get; set; } = [];
    public int ReleaseYear { get; set; }
    public List<Cassette> Cassettes { get; set; } = [];
}
