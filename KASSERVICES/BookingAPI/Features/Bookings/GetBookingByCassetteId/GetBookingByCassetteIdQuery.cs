using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingByCassetteId;

public record GetBookingByCassetteIdQuery(Guid Id) : IRequest<List<BookingDetailsDto>>;
