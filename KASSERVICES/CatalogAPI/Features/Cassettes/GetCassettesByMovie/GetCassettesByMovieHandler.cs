using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Cassettes.GetCassettesByMovie;

public class GetCassettesByMovieHandler(CatalogDbContext dbContext)
    : IRequestHandler<GetCassettesByMovieQuery, List<CassetteDto>>
{
    public async Task<List<CassetteDto>> Handle(GetCassettesByMovieQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cassettes.AsNoTracking().Where(c => c.MovieId == request.MovieId)
            .Select(c => new CassetteDto(
                c.Id,
                c.MovieId,
                c.InventoryNumber,
                (int)c.Status,
                c.CreatedAt
                )).ToListAsync(cancellationToken);
    }
}
