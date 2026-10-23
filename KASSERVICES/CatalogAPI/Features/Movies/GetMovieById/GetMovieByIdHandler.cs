using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.GetMovieById;

public class GetMovieByIdHandler(CatalogDbContext dbContext)
    : IRequestHandler<GetMovieByIdQuery, MovieDetailsDto?>
{
    public async Task<MovieDetailsDto?> Handle(
        GetMovieByIdQuery request,
        CancellationToken cancellationToken)
    {

        return await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.Id == request.Id)
            .Select(m => new MovieDetailsDto(
                m.Id,
                m.Title,
                m.Genres.Select(g => g.Name).ToList(),
                m.ReleaseYear,
                m.Cassettes.Select(c => new CassetteBriefDto(
                    c.Id,
                    c.InventoryNumber,
                    c.Status.ToString()
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}