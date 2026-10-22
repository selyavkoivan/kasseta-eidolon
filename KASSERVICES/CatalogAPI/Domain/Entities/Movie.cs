namespace CatalogAPI.Domain.Entities;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public List<Genre> Genres { get; set; } = [];
    public int ReleaseYear { get; set; }
    public List<Cassette> Cassettes { get; set; } = [];
    public bool IsArchived { get; private set; }

    public void Archive()
    {
        if (IsArchived) return;
        IsArchived = true;
    }

    public void Restore()
    {
        if (!IsArchived) return;
        IsArchived = false;
    }
}
