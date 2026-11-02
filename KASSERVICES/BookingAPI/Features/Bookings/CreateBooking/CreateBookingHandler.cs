using BookingAPI.Data;
using BookingAPI.Domain.Entities;
using BookingAPI.Domain.Enums;
using MassTransit;
using MediatR;
using Shared.Contracts;

namespace BookingAPI.Features.Bookings.CreateBooking;

public class CreateBookingHandler(BookingDbContext dbContext, IRequestClient<CassetteBookingRequested> requestClient)
    : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            CassetteId = request.CassetteId,
            MovieId = request.MovieId,
            DueDate = DateTimeOffset.UtcNow.AddDays(request.DaysToRent),
            Status = BookingStatus.Pending
        };

        Response<CassetteBookingConfirmed, CassetteBookingRejected> response;
        try
        {
            response = await requestClient.GetResponse<
                CassetteBookingConfirmed,
                CassetteBookingRejected>(
                new CassetteBookingRequested(
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

        if (response.Is(out Response<CassetteBookingRejected>? rejected))
        {
            throw new InvalidOperationException(rejected.Message.Reason);
        }

        booking.Status = BookingStatus.Active;
        dbContext.Bookings.Add(booking);
        await dbContext.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}