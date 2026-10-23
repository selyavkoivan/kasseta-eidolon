using CatalogAPI.Common.Interfaces;
using CatalogAPI.Data;
using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class GenreResolver(CatalogDbContext dbContext) : IGenreResolver
{

    public async Task<List<Genre>> ResolveAsync(
        IEnumerable<string>? genreNames,
        CancellationToken cancellationToken = default)
    {
        var normalized = (genreNames ?? Enumerable.Empty<string>())
            .Where(g => !string.IsNullOrWhiteSpace(g))
            .Select(g => g.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalized.Count == 0)
            return [];
        var exisвting = dbContext.Genres.ToList();
        var existing = await dbContext.Genres
            .Where(g => normalized.Contains(g.Name))
            .ToListAsync(cancellationToken);

        var existingNames = existing
            .Select(g => g.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var newOnes = normalized
            .Where(n => !existingNames.Contains(n))
            .Select(n => new Genre { Id = Guid.NewGuid(), Name = n })
            .ToList();

        if (newOnes.Count > 0)
            dbContext.Genres.AddRange(newOnes);

        return [.. existing, .. newOnes];
    }
}