using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Domain.Entities;

public class CassetteProjection
{
    [Key]
    public Guid CassetteId { get; set; }

    public Guid MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;
    public string InventoryNumber { get; set; } = string.Empty;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}