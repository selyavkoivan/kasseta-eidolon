using MediatR;

namespace CatalogAPI.Features.Movies.RestoreMovie;

public record RestoreMovieCommand(Guid Id) : IRequest<Guid>;
