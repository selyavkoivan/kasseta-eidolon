using MediatR;

namespace BookingAPI.Features.Bookings.ReturnBooking;

public record ReturnBookingCommand(Guid Id) : IRequest<Guid>;
