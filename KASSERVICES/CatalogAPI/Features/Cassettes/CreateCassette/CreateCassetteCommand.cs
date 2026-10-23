using MediatR;

namespace CatalogAPI.Features.Cassettes.CreateCassette;

public record CreateCassetteCommand(Guid MovieId, string InventoryNumber, int Status) : IRequest<Guid>;
