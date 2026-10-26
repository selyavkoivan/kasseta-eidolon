using MediatR;

namespace BookingAPI.Features.Bookings.CreateBooking;

public record CreateBookingCommand(Guid CassetteId, Guid MovieId, int DaysToRent = 7) : IRequest<Guid>;