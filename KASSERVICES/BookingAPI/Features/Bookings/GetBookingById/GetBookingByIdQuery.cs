using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingById;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDetailsDto?>;
