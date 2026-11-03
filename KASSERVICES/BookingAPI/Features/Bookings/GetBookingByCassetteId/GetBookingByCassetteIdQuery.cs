using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingById;

public record GetBookingByCassetteIdQuery(Guid Id) : IRequest<List<BookingDetailsDto>>;
