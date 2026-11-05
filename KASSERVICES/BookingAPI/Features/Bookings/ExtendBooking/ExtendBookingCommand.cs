using MediatR;

namespace BookingAPI.Features.Bookings.ExtendBooking;

public record ExtendBookingCommand(Guid Id, DateTimeOffset DueDateExtension) : IRequest<BookingDetailsDto>;
