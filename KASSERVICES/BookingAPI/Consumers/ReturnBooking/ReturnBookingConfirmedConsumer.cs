using BookingAPI.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Consumers.ReturnBooking;

public class ReturnBookingConfirmedConsumer(BookingDbContext db) : IConsumer<CassetteReturnConfirmed>
{
    public async Task Consume(ConsumeContext<CassetteReturnConfirmed> context)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == context.Message.BookingId);
        if (booking == null) return;

        booking.Confirm();

        await db.SaveChangesAsync();
    }
}