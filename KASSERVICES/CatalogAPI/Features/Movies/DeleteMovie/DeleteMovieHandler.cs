using CatalogAPI.Common.Interfaces;
using CatalogAPI.Data;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Features.Movies.CreateMovie;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.DeleteMovie;

public class DeleteMovieHandler(CatalogDbContext dbContext) : IRequestHandler<DeleteMovieCommand, Guid>
{
    public async Task<Guid> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {

        var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                        ?? throw new InvalidOperationException("Movie is not found");

        if(!movie.IsArchived)
        {
            throw new InvalidOperationException("Only archived movies can be deleted. Please archive the movie first.");
        }

        dbContext.Movies.Remove(movie);
        await dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
