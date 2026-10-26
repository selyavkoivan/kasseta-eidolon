using MediatR;

namespace BookingAPI.Features.Bookings.GetBookingById;
public record BookingDetailsDto(
    Guid Id,
    Guid CassetteId,
    Guid MovieId,
    DateTimeOffset BookedAt,
    DateTimeOffset DueDate,
    string Status);

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDetailsDto?>;
