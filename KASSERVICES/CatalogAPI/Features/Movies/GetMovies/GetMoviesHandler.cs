using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.GetMovies;

public class GetMoviesHandler(CatalogDbContext dbContext)
    : IRequestHandler<GetMoviesQuery, List<MovieDto>>
{
    public async Task<List<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Movies.AsNoTracking()
            .Select(m => new MovieDto(
                m.Id,
                m.Title,
                m.Genres.Select(g => g.Name).ToList(),
                m.ReleaseYear,
                m.Cassettes.Count
                )).ToListAsync(cancellationToken);
    }
}