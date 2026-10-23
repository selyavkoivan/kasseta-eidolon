using MediatR;

namespace CatalogAPI.Features.Cassettes.DeleteCassette;

public record DeleteCassetteCommand(Guid CassetteId) : IRequest<Guid>;
