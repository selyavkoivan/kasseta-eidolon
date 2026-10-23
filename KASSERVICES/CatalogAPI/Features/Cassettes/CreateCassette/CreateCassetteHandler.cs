using CatalogAPI.Data;
using CatalogAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Cassettes.CreateCassette;

public class CreateCassetteHandler(CatalogDbContext dbContext) : IRequestHandler<CreateCassetteCommand, Guid>
{
    public async Task<Guid> Handle(CreateCassetteCommand request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken)
                        ?? throw new InvalidOperationException("Movie is not found");

        Cassette cassette = new()
        {
            Id = Guid.NewGuid(),
            MovieId = request.MovieId,
            Movie = movie,
            InventoryNumber = request.InventoryNumber,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Cassettes.AddAsync(cassette, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return cassette.Id;
    }
}
