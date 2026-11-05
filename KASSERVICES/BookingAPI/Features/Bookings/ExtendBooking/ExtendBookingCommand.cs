using MediatR;

namespace BookingAPI.Features.Bookings.ExtendBooking;

public record DateOffset(int Months = 0, int Days = 0);

public record ExtendBookingCommand(Guid Id, int Months, int Days) : IRequest<BookingDetailsDto>;
