using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.RestoreMovie;

public class RestoreMovieHandler(CatalogDbContext dbContext) : IRequestHandler<RestoreMovieCommand, Guid>
{
    public async Task<Guid> Handle(RestoreMovieCommand request, CancellationToken cancellationToken)
    {

        var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                        ?? throw new InvalidOperationException("Movie is not found"); ;

        movie.Restore();
        await dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
