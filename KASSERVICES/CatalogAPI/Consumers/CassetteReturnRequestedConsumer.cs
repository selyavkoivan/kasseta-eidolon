using CatalogAPI.Data;
using CatalogAPI.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace CatalogAPI.Consumers;

public class CassetteReturnRequestedConsumer(CatalogDbContext db)
    : IConsumer<CassetteReturnRequested>
{
    public async Task Consume(ConsumeContext<CassetteReturnRequested> context)
    {
        var msg = context.Message;

        var cassette = await db.Cassettes.FirstOrDefaultAsync(c => c.Id == msg.CassetteId);

        if (cassette == null || cassette.Status == CassetteStatus.Available)
        {
            await context.RespondAsync(new CassetteReturnRejected(
                msg.BookingId,
                msg.CassetteId,
                "Cassette can not be returned."));
            return;
        }

        if (cassette.Status == CassetteStatus.Rented)
        {
            cassette.Status = CassetteStatus.Available;
        }
        await db.SaveChangesAsync();

        await context.RespondAsync(new CassetteReturnConfirmed(
            msg.BookingId,
            msg.CassetteId,
            DateTimeOffset.UtcNow));
    }
}

