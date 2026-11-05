using CatalogAPI.Data;
using CatalogAPI.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace CatalogAPI.Consumers
{
    public class BookingExtendRequestConsumer(CatalogDbContext dbContext) : IConsumer<BookingExtendRequested>
    {
        public async Task Consume(ConsumeContext<BookingExtendRequested> context)
        {
            var msg = context.Message;

            var cassette = await dbContext.Cassettes.FirstOrDefaultAsync(c => c.Id == msg.CassetteId);

            if (cassette?.Status == CassetteStatus.Rented)
            {
                await context.RespondAsync(new BookingExtendConfirmed(
                        msg.BookingId,
                        msg.CassetteId,
                        DateTimeOffset.UtcNow));
            }

            await context.RespondAsync(new BookingExtendRejected(
                    msg.BookingId,
                    msg.CassetteId,
                    "Cassette is not rented."));
        }
    }
}
