using MediatR;

namespace CatalogAPI.Features.Cassettes.ChangeCassetteStatus;

public record ChangeCassetteStatusCommand(Guid CassetteId, int Status) : IRequest<Guid>;
