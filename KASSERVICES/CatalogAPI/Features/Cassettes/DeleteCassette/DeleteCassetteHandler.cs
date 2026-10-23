using CatalogAPI.Data;
using CatalogAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Cassettes.DeleteCassette;

public class DeleteCassetteHandler(CatalogDbContext dbContext) : IRequestHandler<DeleteCassetteCommand, Guid>
{
    public async Task<Guid> Handle(DeleteCassetteCommand request, CancellationToken cancellationToken)
    {
        var cassette = await dbContext.Cassettes.FirstOrDefaultAsync(m => m.Id == request.CassetteId, cancellationToken)
                        ?? throw new InvalidOperationException("Cassette is not found");

        if (cassette.Status is CassetteStatus.Rented)
        {
            throw new InvalidOperationException("Rented cassettes can not be deleted. Please change the cassette status.");
        }

        dbContext.Cassettes.Remove(cassette);
        await dbContext.SaveChangesAsync(cancellationToken);

        return cassette.Id;
    }
}
