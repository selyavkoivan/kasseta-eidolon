using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingByMovieId;

public record GetBookingByMovieIdQuery(Guid Id) : IRequest<List<BookingDetailsDto>>;
