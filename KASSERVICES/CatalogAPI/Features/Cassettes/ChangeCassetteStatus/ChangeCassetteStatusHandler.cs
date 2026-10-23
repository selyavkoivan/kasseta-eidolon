using CatalogAPI.Data;
using CatalogAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Cassettes.ChangeCassetteStatus;

public class ChangeCassetteStatusHandler(CatalogDbContext dbContext) : IRequestHandler<ChangeCassetteStatusCommand, Guid>
{
    public async Task<Guid> Handle(ChangeCassetteStatusCommand request, CancellationToken cancellationToken)
    {
        var cassette = await dbContext.Cassettes.FirstOrDefaultAsync(c => c.Id == request.CassetteId, cancellationToken)
            ?? throw new InvalidOperationException("Cassette is not found");

        cassette.Status = (CassetteStatus)request.Status;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return cassette.Id;
    }
}
