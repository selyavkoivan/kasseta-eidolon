using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers;

public class ReturnBookingRejectedConsumer(BookingDbContext db, ILogger<ReturnBookingRejectedConsumer> logger)
    : IConsumer<CassetteReturnRejected>
{
    public async Task Consume(ConsumeContext<CassetteReturnRejected> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        logger.LogWarning("Booking {BookingId} return rejected: {Reason}", booking.Id, context.Message.Reason);

        booking.Reject();
        await db.SaveChangesAsync();
    }
}