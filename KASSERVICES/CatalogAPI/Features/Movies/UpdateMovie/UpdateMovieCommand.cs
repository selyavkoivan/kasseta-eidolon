using MediatR;

namespace CatalogAPI.Features.Movies.UpdateMovie;

public record UpdateMovieCommand(Guid Id, string Title, List<string> Genres, int ReleaseYear) : IRequest<Guid>;
