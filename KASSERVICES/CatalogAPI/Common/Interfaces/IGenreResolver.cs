using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Common.Interfaces;

public interface IGenreResolver
{
    Task<List<Genre>> ResolveAsync(
        IEnumerable<string>? genreNames,
        CancellationToken cancellationToken = default);
}