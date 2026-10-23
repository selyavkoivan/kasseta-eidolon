using MediatR;

namespace CatalogAPI.Features.Movies.GetMovieById;

public record MovieDetailsDto(
    Guid Id,
    string Title,
    List<string> Genres,
    int ReleaseYear,
    List<CassetteBriefDto> Cassettes
);

public record CassetteBriefDto(
    Guid Id,
    string InventoryNumber,
    string Status
);

public record GetMovieByIdQuery(Guid Id) : IRequest<MovieDetailsDto?>;