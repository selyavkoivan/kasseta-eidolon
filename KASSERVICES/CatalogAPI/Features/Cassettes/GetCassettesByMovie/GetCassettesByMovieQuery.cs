using CatalogAPI.Domain.Enums;
using CatalogAPI.Features.Movies.GetMovieById;
using MediatR;

namespace CatalogAPI.Features.Cassettes.GetCassettesByMovie;

public record CassetteDto(
    Guid Id,
    Guid MovieId,
    string InventoryNumber,
    int Status,
    DateTime CreatedAt
);

public record GetCassettesByMovieQuery(Guid MovieId) : IRequest<List<CassetteDto>>;
