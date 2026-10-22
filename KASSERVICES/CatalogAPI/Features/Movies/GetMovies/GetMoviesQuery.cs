using MediatR;

namespace CatalogAPI.Features.Movies.GetMovies;

public record MovieDto(
    Guid Id,
    string Title,
    List<string> Genres,
    int ReleaseYear,
    int CassettesCount
);

public record GetMoviesQuery() : IRequest<List<MovieDto>>;
