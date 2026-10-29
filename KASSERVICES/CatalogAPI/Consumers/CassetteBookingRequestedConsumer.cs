using CatalogAPI.Data;
using CatalogAPI.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace CatalogAPI.Consumers;

public class CassetteBookingRequestedConsumer(CatalogDbContext db)
    : IConsumer<CassetteBookingRequested>
{
    public async Task Consume(ConsumeContext<CassetteBookingRequested> context)
    {
        var msg = context.Message;

        var cassette = await db.Cassettes.FirstOrDefaultAsync(c => c.Id == msg.CassetteId);

        if (cassette == null || cassette.Status != CassetteStatus.Available)
        {
            await context.Publish(new CassetteBookingRejected(
                msg.BookingId,
                msg.CassetteId,
                "Cassette is not available."));
            return;
        }

        cassette.Status = CassetteStatus.Rented;
        await db.SaveChangesAsync();

        await context.Publish(new CassetteBookingConfirmed(
            msg.BookingId,
            msg.CassetteId,
            DateTimeOffset.UtcNow));
    }
}
