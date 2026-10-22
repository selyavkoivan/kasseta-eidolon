using MediatR;

namespace CatalogAPI.Features.Movies.ArchiveMovie;

public record ArchiveMovieCommand(Guid Id) : IRequest<Guid>;
