using BookingAPI.Data;
using BookingAPI.Domain.Enums;
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


        if (booking.Status != BookingStatus.Active && booking.Status != BookingStatus.Overdue)
            throw new InvalidOperationException("Only active bookings can be extendet.");

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

        if (booking.DueDate <= DateTimeOffset.UtcNow)
            booking.Status = BookingStatus.Active;

        await dbContext.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
