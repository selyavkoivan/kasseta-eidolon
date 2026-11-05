using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers.Extend;

public class BookingExtendConfirmedConsumer(BookingDbContext db) : IConsumer<BookingExtendConfirmed>
{
    public async Task Consume(ConsumeContext<BookingExtendConfirmed> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        booking.Confirm();

        await db.SaveChangesAsync();
    }
}