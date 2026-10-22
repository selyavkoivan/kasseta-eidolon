using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.ArchiveMovie;

public class ArchiveMovieHandler(CatalogDbContext dbContext) : IRequestHandler<ArchiveMovieCommand, Guid>
{
    public async Task<Guid> Handle(ArchiveMovieCommand request, CancellationToken cancellationToken)
    {

        var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                        ?? throw new InvalidOperationException("Movie is not found"); ;

        movie.Archive();
        await dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
