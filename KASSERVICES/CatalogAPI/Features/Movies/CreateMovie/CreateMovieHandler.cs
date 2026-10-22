using CatalogAPI.Common.Interfaces;
using CatalogAPI.Data;
using CatalogAPI.Domain.Entities;
using MediatR;

namespace CatalogAPI.Features.Movies.CreateMovie;

public class CreateMovieHandler(CatalogDbContext dbContext, IGenreResolver genreResolver) : IRequestHandler<CreateMovieCommand, Guid>
{
    public async Task<Guid> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {

        var allGenres = await genreResolver.ResolveAsync(request.Genres, cancellationToken);

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Genres = allGenres,
            ReleaseYear = request.ReleaseYear,
        };

        dbContext.Movies.Add(movie);
        await dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}
