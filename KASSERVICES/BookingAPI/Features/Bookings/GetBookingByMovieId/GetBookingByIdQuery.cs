using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingById;

public record GetBookingByMovieIdQuery(Guid Id) : IRequest<List<BookingDetailsDto>>;
