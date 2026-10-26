using BookingAPI.Data;
using BookingAPI.Domain.Entities;
using BookingAPI.Domain.Enums;
using MassTransit;
using MediatR;
using Shared.Contracts;

namespace BookingAPI.Features.Bookings.CreateBooking;

public class CreateBookingHandler(BookingDbContext dbContext, IPublishEndpoint publisher)
    : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            CassetteId = request.CassetteId,
            MovieId = request.MovieId,
            DueDate = DateTimeOffset.UtcNow.AddDays(request.DaysToRent),
            Status = BookingStatus.Pending
        };

        dbContext.Bookings.Add(booking);

        await publisher.Publish(new CassetteBookingRequested(
            booking.Id,
            booking.CassetteId,
            DateTimeOffset.UtcNow), cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}