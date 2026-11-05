using BookingAPI.Data;
using BookingAPI.Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace BookingAPI.Features.Bookings.ReturnBooking;

public class ReturnBookingHandler(BookingDbContext dbContext, IRequestClient<CassetteReturnRequested> requestClient)
    : IRequestHandler<ReturnBookingCommand, BookingDetailsDto>
{
    public async Task<BookingDetailsDto> Handle(ReturnBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Booking is not found"); ;


        Response<CassetteReturnConfirmed, CassetteReturnRejected> response;
        try
        {
            response = await requestClient.GetResponse<
                CassetteReturnConfirmed,
                CassetteReturnRejected>(
                new CassetteReturnRequested(
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

        if (response.Is(out Response<CassetteReturnRejected>? rejected))
        {
            throw new InvalidOperationException(rejected.Message.Reason);
        }
        booking.Status = BookingStatus.Returned;
        await dbContext.SaveChangesAsync(cancellationToken);

        return booking.ToDetailsDto();
    }
}
