using MediatR;

namespace CatalogAPI.Features.Movies.CreateMovie;

public record CreateMovieCommand(string Title, List<string> Genres, int ReleaseYear) : IRequest<Guid>;