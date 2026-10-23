using MediatR;

namespace CatalogAPI.Features.Movies.DeleteMovie;

public record DeleteMovieCommand(Guid Id) : IRequest<Guid>;
