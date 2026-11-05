using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers.Extend;

public class BookingExtendRejectedConsumer(BookingDbContext db, ILogger<BookingExtendRejectedConsumer> logger)
    : IConsumer<BookingExtendRejected>
{
    public async Task Consume(ConsumeContext<BookingExtendRejected> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        logger.LogWarning("Booking {BookingId} extend rejected: {Reason}", booking.Id, context.Message.Reason);

        booking.Reject();
        await db.SaveChangesAsync();
    }
}