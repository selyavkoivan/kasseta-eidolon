using BookingAPI.Data;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Features.Bookings.ExtendBooking;

public class ExtendBookingHandler(BookingDbContext dbContext, IRequestClient<CassetteReturnRequested> requestClient) : IRequestHandler<ExtendBookingCommand, BookingDetailsDto>
{
    public async Task<BookingDetailsDto> Handle(ExtendBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
           ?? throw new InvalidOperationException("Booking is not found");


        Response<BookingExtendConfirmed, BookingExtendRejected> response;
        try
        {
            response = await requestClient.GetResponse<
                BookingExtendConfirmed,
                BookingExtendRejected>(
                new BookingExtendRequested(
                    booking.Id,
                    booking.CassetteId,
                    DateTimeOffset.UtcNow),
                cancellationToken,
                timeout: TimeSpan.FromSeconds(10));
        }
        catch (RequestTimeoutException)
        {
            throw new InvalidOperationException("Catalog service did not respond in time.");
        }

        if (response.Is(out Response<BookingExtendRejected>? rejected))
        {
            throw new InvalidOperationException(rejected.Message.Reason);
        }

        booking.DueDate = booking.DueDate
                .AddYears(request.DueDateExtension.Year)
                .AddMonths(request.DueDateExtension.Month)
                .AddDays(request.DueDateExtension.Day);

        await dbContext.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
