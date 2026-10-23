using CatalogAPI.Common.Interfaces;
using CatalogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Movies.UpdateMovie;

public class UpdateMovieHandler(CatalogDbContext dbContext, IGenreResolver genreResolver) : IRequestHandler<UpdateMovieCommand, Guid>
{
    public async Task<Guid> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {

        var allGenres = await genreResolver.ResolveAsync(request.Genres, cancellationToken);

        var movie = await dbContext.Movies
                        .Include(m => m.Genres)
                        .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) 
                        ?? throw new InvalidOperationException("Movie is not found");

        movie.Title = request.Title;
        movie.ReleaseYear = request.ReleaseYear;
        movie.Genres = allGenres;

        await dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
