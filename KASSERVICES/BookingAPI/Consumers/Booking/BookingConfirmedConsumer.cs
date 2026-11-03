using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers.Booking;

public class BookingConfirmedConsumer(BookingDbContext db) : IConsumer<CassetteBookingConfirmed>
{
    public async Task Consume(ConsumeContext<CassetteBookingConfirmed> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        booking.Confirm();

        await db.SaveChangesAsync();
    }
}