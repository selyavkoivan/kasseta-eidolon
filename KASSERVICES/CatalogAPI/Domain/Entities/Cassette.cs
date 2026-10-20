using CatalogAPI.Domain.Enums;

namespace CatalogAPI.Domain.Entities;
public class Cassette
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public string InventoryNumber { get; set; } = string.Empty;
    public CassetteStatus Status { get; set; } = CassetteStatus.Available;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
