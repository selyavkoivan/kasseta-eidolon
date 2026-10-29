using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers;

public class BookingRejectedConsumer(BookingDbContext db, ILogger<BookingRejectedConsumer> logger)
    : IConsumer<CassetteBookingRejected>
{
    public async Task Consume(ConsumeContext<CassetteBookingRejected> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        logger.LogWarning("Booking {BookingId} rejected: {Reason}", booking.Id, context.Message.Reason);

        booking.Reject();
        await db.SaveChangesAsync();
    }
}